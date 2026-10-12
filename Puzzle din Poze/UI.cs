using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle_din_Poze // Numele aplicatiei
{
    public partial class UI : Form // Clasa principala a ferestrei
    {
        //VARIABILE GLOBALE 
        List<PictureBox> lista_poze = new List<PictureBox>(); // Lista cu toate patratelele din puzzle
        List<Bitmap> poze = new List<Bitmap>(); // Lista cu imaginile decupate din poza originala
        List<string> locatii = new List<string>(); // Ordinea corecta a pieselor
        List<string> locatii_curente = new List<string>(); // Ordinea actuala a pieselor dupa miscari

        string pozitie_castigatoare; // Ordinea pieselor atunci cand puzzle-ul e complet
        string pozitie_curenta; // Ordinea curenta a pieselor
        int secunde = 0; // Timer-ul jocului

        Bitmap poza_principala; // Imaginea selectata de utilizator
        // ---------- CONSTRUCTOR FORM ----------
        public UI()
        {
            InitializeComponent(); // Initializeaza componentele vizuale ale formularului
        }

        //FUNCTIE DE INCARCARE IMAGINE
        public void Buton_Fisier(object sender, EventArgs e)
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            // Daca exista piese incarcate anterior, le stergem si resetam totul
            if (lista_poze != null)
            {
                foreach (PictureBox poza in lista_poze.ToList())
                {
                    this.Controls.Remove(poza); // Eliminam piesele de pe ecran
                }
                lista_poze.Clear(); // Curatam lista de PictureBox-uri
                poze.Clear(); // Curatam lista de imagini
                locatii.Clear(); // Curatam ordinea corecta
                locatii_curente.Clear(); // Curatam ordinea curenta
                pozitie_castigatoare = string.Empty;
                pozitie_curenta = string.Empty;
                label2.Text = string.Empty; // Resetam textul de status
            }

            // Dialog pentru selectarea unei imagini
            OpenFileDialog deschidere = new OpenFileDialog();
            deschidere.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Filtram doar imagini

            if (deschidere.ShowDialog() == DialogResult.OK) // Daca s-a ales o imagine
            {
                poza_principala = new Bitmap(deschidere.FileName); // Salvam imaginea aleasa
                Creare_PictureBox(); // Initializam patratelele puzzle-ului
                adaugare_imagine(); // Taiem si plasam imaginile in patratele
            }
        }

        //CREARE PATRATELE PUZZLE
        private void Creare_PictureBox()
        {
            for (int i = 0; i < 9; i++) // 9 piese pentru puzzle
            {
                PictureBox poza_temporara = new PictureBox(); // Cream o patratica noua
                poza_temporara.Size = new Size(130, 130); // Dimensiune fixa
                poza_temporara.Tag = i.ToString(); // Tag-ul e indexul, folosit pentru verificare
                poza_temporara.Click += ApasarePoza; // Eveniment de click pe patratica
                lista_poze.Add(poza_temporara); // Adaugam in lista
                locatii.Add(poza_temporara.Tag.ToString()); // Salvam ordinea corecta
            }

            secunde = 0; // Resetam timpul
            label5.Text = "00:00"; // Afisam 00:00
            timer1.Start(); // Pornim cronometrul
        }

        //GESTIONAREA MUTARILOR
        private void ApasarePoza(object sender, EventArgs e)
        {
            PictureBox bucata = (PictureBox)sender; // Piesa apasata
            PictureBox bucata_goala = lista_poze.Find(x => x.Tag.ToString() == "0"); //Cautam piesa goala

            // Retinem locatiile pieselor
            Point poza1 = new Point(bucata.Location.X, bucata.Location.Y);
            Point poza2 = new Point(bucata_goala.Location.X, bucata_goala.Location.Y);

            // Pozitiile in lista de controale
            var index1 = this.Controls.IndexOf(bucata);
            var index2 = this.Controls.IndexOf(bucata_goala);

            // Verificam daca cele doua piese sunt adiacente
            if ((bucata.Right == bucata_goala.Left && bucata.Location.Y == bucata_goala.Location.Y)
                || (bucata.Left == bucata_goala.Right && bucata.Location.Y == bucata_goala.Location.Y)
                || (bucata.Top == bucata_goala.Bottom && bucata.Location.X == bucata_goala.Location.X)
                || (bucata.Bottom == bucata_goala.Top && bucata.Location.X == bucata_goala.Location.X))
            {
                bucata.Location = poza2; // Schimbam pozitia vizual
                bucata_goala.Location = poza1;

                // Interschimbam ordinea in controale
                this.Controls.SetChildIndex(bucata, index2);
                this.Controls.SetChildIndex(bucata_goala, index1);
            }

            label2.Text = ""; // Resetam textul de stare
            locatii_curente.Clear(); // Golim lista curenta
            verificare_joc(); // Verificam daca puzzle-ul e complet
        }

        // TAIERE IMAGINE IN BUCATI
        private void Taiere_Poza(Bitmap principal, int inaltime, int latime)
        {
            int x = 0, y = 0;

            for (int bloc = 0; bloc < 9; bloc++) // 9 piese
            {
                Bitmap poza_taiata = new Bitmap(inaltime, latime); // Cream o imagine noua

                for (int i = 0; i < inaltime; i++)
                    for (int j = 0; j < latime; j++)
                    {
                        poza_taiata.SetPixel(j, i, principal.GetPixel((j + x), (i + y))); // Copiem pixelii (j = X, i = Y)
                    }

                poze.Add(poza_taiata); // Adaugam imaginea decupata in lista
                x += 130;
                if (x == 390) { x = 0; y += 130; } // Trecem pe randul urmator
            }
        }

        //ADAUGARE IMAGINE IN PUZZLE
        private void adaugare_imagine()
        {
            Bitmap temporar = new Bitmap(poza_principala, new Size(390, 390)); // Redimensionam poza

            Taiere_Poza(temporar, 130, 130); // Taiem in bucati

            for (int i = 1; i < lista_poze.Count; i++) // Nu punem poza pe piesa goala (index 0)
            {
                lista_poze[i].BackgroundImage = (Image)poze[i];
            }

            Aplicare_PictureBox(); // Aranjam piesele pe ecran

            label6.Visible = false; // Ascundem mesajul final
            pictureBox1.Visible = false; // Ascundem gif-ul
        }

        //ARANJARE SI RANDOMIZARE PATRATELE
        private void Aplicare_PictureBox()
        {
            var randombuc = lista_poze.OrderBy(a => Guid.NewGuid()).ToList(); // Amestecam piesele
            lista_poze = randombuc;

            // Garantam ca puzzle-ul este solvabil (numar par de inversiuni)
            int inversiuni = 0;
            var pieseFaraZero = lista_poze.Where(p => p.Tag.ToString() != "0").ToList();
            for (int i = 0; i < pieseFaraZero.Count - 1; i++)
            {
                for (int j = i + 1; j < pieseFaraZero.Count; j++)
                {
                    if (int.Parse(pieseFaraZero[i].Tag.ToString()) > int.Parse(pieseFaraZero[j].Tag.ToString()))
                        inversiuni++;
                }
            }
            if (inversiuni % 2 != 0)
            {
                int idx1 = lista_poze.FindIndex(p => p.Tag.ToString() != "0");
                int idx2 = lista_poze.FindIndex(idx1 + 1, p => p.Tag.ToString() != "0");
                var temp = lista_poze[idx1];
                lista_poze[idx1] = lista_poze[idx2];
                lista_poze[idx2] = temp;
            }

            pozitie_castigatoare = string.Join("", locatii); // Ordinea corecta a pieselor ("012345678")

            int x = 200, y = 25;

            for (int i = 0; i < lista_poze.Count; i++)
            {
                lista_poze[i].BackColor = Color.Gold; // Fundal
                if (i == 3 || i == 6)
                {
                    x = 200;
                    y += 130; // Trecem la randul urmator
                }

                lista_poze[i].BorderStyle = BorderStyle.FixedSingle;
                lista_poze[i].Location = new Point(x, y); // Setam pozitia pe ecran
                this.Controls.Add(lista_poze[i]); // Adaugam in formular
                x += 130;
            }
        }

        //VERIFICARE DACA PUZZLE-UL E COMPLET
        private void verificare_joc()
        {
            // Parcurgem piesele ordonate dupa pozitia lor vizuala pe ecran (de la stanga la dreapta, de sus in jos)
            foreach (var pb in lista_poze.OrderBy(p => p.Location.Y).ThenBy(p => p.Location.X))
            {
                locatii_curente.Add(pb.Tag.ToString()); // Construim lista curenta
            }

            pozitie_curenta = string.Join("", locatii_curente); // Convertim in string
            label1.Text = pozitie_castigatoare;
            label2.Text = pozitie_curenta;

            if (pozitie_curenta == pozitie_castigatoare)
            {
                timer1.Stop(); // Oprim cronometrul
                TimeSpan t = TimeSpan.FromSeconds(secunde); // Calculam timpul
                label2.Text = "";

                label6.Text = $"Felicitări!\nTimp: {t:mm\\:ss}";
                label6.Visible = true;
                pictureBox1.Visible = true; // Afisam gif-ul
            }
        }

        // CRONOMETRU
        private void timer1_Tick(object sender, EventArgs e)
        {
            secunde++; // Incrementam secundele
            TimeSpan t = TimeSpan.FromSeconds(secunde);
            label5.Text = t.ToString(@"mm\:ss"); //Format mm:ss
        }

        // RESETARE COMPLETA
        private void button1_Click(object sender, EventArgs e)
        {
            // Reafisam label-urile
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = false;
            pictureBox1.Visible = false;

            if (poza_principala == null)
            {
                MessageBox.Show("Trebuie să alegi o imagine înainte de a reseta jocul.");
                return;
            }

            // Resetam tot
            timer1.Stop();
            foreach (PictureBox poza in lista_poze.ToList())
            {
                this.Controls.Remove(poza);
            }
            lista_poze.Clear();
            poze.Clear();
            locatii.Clear();
            locatii_curente.Clear();
            pozitie_castigatoare = "";
            pozitie_curenta = "";
            label1.Text = "";
            label2.Text = "";
            secunde = 0;
            label5.Text = "00:00";

            Creare_PictureBox(); // Reinitializam jocul
            adaugare_imagine();
            timer1.Start(); // Repornim timerul
        }

        //INCHIDERE APLICATIE
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Inchide aplicatia
        }

        //INCEPERE JOC (BUTTON "Start")
        private void button3_Click(object sender, EventArgs e)
        {
            // Resetare label-uri
            label1.Text = ""; // pentru ordinea castigatoare
            label2.Text = ""; // pentru ordinea curenta
            label5.Text = "00:00"; // pentru timpul scurs
            label6.Visible = false; // ascunde mesajul de final
            pictureBox1.Visible = false; // ascunde gif-ul daca era pornit

            secunde = 0; // resetam secundele
            timer1.Stop(); // oprim timerul daca era activ

            // Apelam logica principala de start joc
            Buton_Fisier(sender, e);
        }
    }
}