# Robot Planet

## Minu projekt

Minu projekt on Robot Planet. See on väike WPF-programm, kus saab roboteid lisada ja nendega erinevaid tegevusi teha.

Programmis saab:
- lisada uue roboti;
- valida roboti nimekirjast;
- eemaldada roboti;
- teha tavalist või naljakat tegevust;
- laadida akut ja skaneerida;
- vaadata roboti energiat ja tegevuste logi.

## Kuidas projekt töötab?

Minu projektis on kaks osa:

- **RobotPlanet.Core** – siin on robotite klassid ja nende tegevused.
- **RobotPlanet.WpfApp** – siin on programmi aken ja nupud.

Core ei sõltu WPF-ist.

## Robotid

Mul on üks abstraktne klass **Robot** ja kolm robotit:

- **CleanerBot** – koristab.
- **ExplorerBot** – uurib planeeti.
- **RepairBot** – parandab teisi roboteid.

Igal robotil on nimi ja energia. Alguses on energiat 100.

Roboti nime ei saa pärast loomist muuta. Energiat saab muuta ainult roboti meetodite kaudu.

## Liidesed

Kasutasin kahte liidest:

- **IChargeable** – aku laadimiseks.
- **IScannable** – skaneerimiseks.

CleanerBot saab akut laadida.

ExplorerBot saab skaneerida.

RepairBot saab teha mõlemat.

Programmis kontrollin `is` abil, mida valitud robot oskab teha.

## Robotite tegevused

Igal robotil on kaks meetodit:

- `NormalAction()` – tavaline tegevus.
- `CrazyAction()` – naljakas tegevus.

Iga robot teeb neid tegevusi omamoodi.

Tegevused kasutavad energiat. Kui energiat ei ole piisavalt, näitab programm veateadet ja energia ei muutu.

RepairBot teeb ka erinevaid naljakaid tegevusi sõltuvalt energiast. Kui pärast tegevust on energiat vähem kui 40, parandab ta kogemata külmkappi.

Aku laadimisel saab robot 20 energiat juurde, aga energiat ei saa olla rohkem kui 100.

## Programmi aken

Programmis on robotite nimekiri, nupud ja tegevuste logi.

Robotite hoidmiseks kasutasin `ObservableCollection<Robot>`.

Uue roboti lisamisel saab kasutaja kirjutada tema nime. Kui nimi on tühi, näitab programm veateadet.

Õppisin kasutama **ProgressBar** elementi. Valisin selle, sest sellega on lihtne näha, kui palju energiat robotil alles on.

Nuppude ja teiste elementide tekstid on failis `Resources.resx`.

## Testimine

Proovisin programmis järgmisi asju:

1. CleanerBot teeb tavalise tegevuse. Energia muutub 100 → 90.
2. CleanerBot teeb naljaka tegevuse. Energia muutub 90 → 70.
3. CleanerBot laeb akut. Energia muutub 70 → 90.
4. ExplorerBot saab skaneerida, aga laadimise nupp ei tööta tema jaoks.
5. RepairBot saab nii laadida kui ka skaneerida.
6. Kui energiat ei ole piisavalt, tuleb veateade ja energia jääb samaks.
7. Roboti eemaldamisel kaob ta nimekirjast.
8. Tühja nimega robotit ei saa lisada.
9. Kui RepairBotil on vähe energiat, muutub tema naljakas tegevus.

## Kuidas programmi käivitada?

1. Ava `RobotPlanet.sln` Visual Studios.
2. Vali `RobotPlanet.WpfApp` käivitusprojektiks.
3. Vajuta F5.

Kasutasin .NET 10 ja WPF-i.

## Koostöö

*Selle osa täidan pärast koostööd klassikaaslasega.*

- Klassikaaslane:
- Issue:
- Pull Request:

## Tehisintellekti kasutamine

Kasutasin ChatGPT-d abivahendina, et paremini mõista C# ja WPF-i ning saada abi koodi kirjutamisel ja vigade parandamisel.

Töötasin projektiga Visual Studios, tegin muudatusi ja kontrollisin ise, kas programm töötab õigesti.