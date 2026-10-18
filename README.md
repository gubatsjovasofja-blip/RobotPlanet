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

Vol.1 versioonis oli mul üks abstraktne klass **Robot** ja neli robotit:

- **CleanerBot** – koristab.
- **ExplorerBot** – uurib planeeti.
- **RepairBot** – parandab teisi roboteid.
- **DanceBot** – tantsib ja korraldab robotidiskot.

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
10. DanceBot teeb tavalise tegevuse. Energia muutub 100 → 90.
11. DanceBot teeb naljaka tegevuse. Energia muutub 90 → 70.
12. Kui DanceBotil on ainult 10 energiat, ei saa ta teha 20 energiat nõudvat tegevust. Programm näitab veateadet ja energia ei muutu.

## Kuidas programmi käivitada?

1. Ava `RobotPlanet.slnx` Visual Studios.
2. Vali `RobotPlanet.WpfApp` käivitusprojektiks.
3. Vajuta F5.

Kasutasin .NET 10 ja WPF-i.

## Koostöö

Tegin projekti koostöös klassikaaslasega.

- **Klassikaaslane:** [P1prka](https://github.com/P1prka)
- **Issue:** [DanceBot #1](https://github.com/gubatsjovasofja-blip/RobotPlanet/issues/1)
- **Pull Request:** [DanceBot #2](https://github.com/gubatsjovasofja-blip/RobotPlanet/pull/2)

Klassikaaslane lisas minu projekti uue roboti DanceBot. Mina kontrollisin Visual Studios, et robot töötab õigesti, ja ühendasin Pull Requesti põhiharuga.

Mina lisasin klassikaaslase CrazyZoo projekti uue looma Monkey.

- **CrazyZoo Issue:** [Monkey #1](https://github.com/P1prka/CrazyZoo/issues/1)
- **CrazyZoo Pull Request:** [Monkey #2](https://github.com/P1prka/CrazyZoo/pull/2)

## Tehisintellekti kasutamine

Kasutasin ChatGPT-d abivahendina, et paremini mõista C# ja WPF-i ning saada abi koodi kirjutamisel ja vigade parandamisel.

Töötasin projektiga Visual Studios, tegin muudatusi ja kontrollisin ise, kas programm töötab õigesti.

## Crazy Universe Vol.2

### Projekti täiendamine

Vol.2 ülesandes jätkasin oma RobotPlanet projektiga ja lisasin uusi võimalusi.

Kasutasin `IRepository<T>` ja `Repository<T>` klasse, et hoida erinevaid robotitüüpe eraldi. Roboteid saab lisada ja eemaldada.

### Sündmused

Kui robot teeb `CrazyAction()` tegevuse, reageerivad sellele teised sama hoidla robotid.

Kui robot eemaldatakse hoidlast, ei reageeri ta enam sündmustele.

Reaktsioonid kuvatakse tegevuste logis ja võivad muuta roboti energiat.

### LINQ

Kasutasin kolme LINQ võimalust:

- `Where()` – robotite filtreerimiseks.
- `OrderBy()` – robotite järjestamiseks.
- `GroupBy()` – robotite rühmitamiseks.

### WPF ja MVVM

Täiendasin programmi MVVM põhimõtte järgi.

Kasutasin `INotifyPropertyChanged`, `ObservableCollection` ja `ICommand`.

Kasutaja saab valida robotite hoidla, lisada ja eemaldada roboteid ning näha nende tegevusi ja energia muutumist.

### Uued robotid

Vol.2 jaoks lisas Ruslana kolm uut robotit:

- **ChefBot** – valmistab süüa.
- **GuardBot** – valvab.
- **FlyingBot** – lendab.

Nüüd on projektis kokku seitse robotitüüpi.

### Koostöö Vol.2

Tegin koostööd klassikaaslase Ruslana Nekrakhaga.

- **Ruslana Pull Request:** [#5 – ChefBot, GuardBot ja FlyingBot](https://github.com/gubatsjovasofja-blip/RobotPlanet/pull/5)

Kontrollisin Ruslana muudatusi ja ühendasin need oma Vol.2 harusse.

Mina lisasin teise klassikaaslase RobotPlanet projekti kolm robotit: FireBot, MusicBot ja GardenBot.

- **Minu Pull Request:** [#4 – FireBot, MusicBot ja GardenBot](https://github.com/soyawo-ut/RobotPlanet/pull/4)

Klassikaaslane kontrollis minu muudatusi ja kiitis need heaks.

### Testimine

Kontrollisin Visual Studios, et projekt kompileerub vigadeta.

Proovisin uute robotite tegevusi, energiakulu ja tegevuste logi. Kontrollisin ka robotite lisamist ja eemaldamist.
