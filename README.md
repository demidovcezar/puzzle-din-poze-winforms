# 🧩 Puzzle din Poze — Joc 8-Puzzle (Windows Forms C#)

> **Proiect de Atestat**  
> Acest proiect a fost dezvoltat pentru **proba practică de proiecte din cadrul Atestatului la Informatică** de la sfârșitul clasei a XII-a.

---

## 📝 Descriere Proiect

Aplicația este o reinterpretare digitală a jocului clasic **8-Puzzle** (sau *Sliding Tile Puzzle*), dezvoltată în C# folosind tehnologia **Windows Forms**. 

Aplicația permite utilizatorului să încarce orice imagine personală din calculator (`.jpg`, `.png`, `.bmp`), pe care o taie automat într-o grilă de **3x3** (9 piese, dintre care una devine spațiu liber). Scopul jocului este de a muta piesele adiacente pentru a reîntregi imaginea inițială în cel mai scurt timp posibil.

---

## ✨ Caracteristici Principale

- **Încărcare imagine personalizată:** Posibilitatea de a alege orice imagine din fișierele locale prin `OpenFileDialog`.
- **Tăiere și scalare automată:** Redimensionare la dimensiunea grilei de joc ($390 \times 390$ pixeli) și decupare automată în 9 bucăți egale.
- **Garanție de solvabilitate:** Algoritmul de amestecare verifică paritatea inversiunilor pentru a asigura că fiecare configurație generată poate fi rezolvată.
- **Cronometru integrat:** Înregistrarea și afișarea timpului scurs în format `mm:ss`.
- **Interfață intuitivă:** Control facil al pieselor prin click pe piesele adiacente spațiului gol.
- **Verificare automată:** Detectarea instantanee a finalizării puzzle-ului și afișarea ecranului de felicitări.

---

## 🛠️ Tehnologii Utilizate

- **Limbaj:** C#
- **Platformă / Cadrul de lucru:** .NET Framework (Windows Forms)
- **IDE recomandat:** Visual Studio 2019 / 2022
- **Librării:** GDI+ (`System.Drawing`) pentru manipularea și decuparea imaginilor

---

## 🚀 Cum se rulează proiectul

1. **Clonarea repository-ului:**
   ```bash
   git clone [https://github.com/UTILIZATORUL-TAU/puzzle-din-poze-winforms.git](https://github.com/UTILIZATORUL-TAU/puzzle-din-poze-winforms.git)
Deschide fișierul soluție .sln în Visual Studio.
Apasă tasta F5 (sau butonul Start) pentru a compila și rula aplicația.

🎮 Ghid de Utilizare
Apasă pe butonul Start Game / Alege Imaginea.
Selectează o imagine din calculator.

Jocul va amesteca piesele și va porni cronometrul.

Dă click pe oricare piesă adiacentă pătratului gol pentru a o muta în acel spațiu.

Reîntregește imaginea pentru a opri timpul și a câștiga jocul!
