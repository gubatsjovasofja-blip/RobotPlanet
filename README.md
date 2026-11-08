# Robot Planet

## Minu projekt

Minu projekt on Robot Planet. See on C# WPF-programm, kus saab roboteid lisada, eemaldada ja nendega erinevaid tegevusi teha.

Alustasin lihtsama robotite programmiga ja täiendasin seda järgmistes kodutöödes.

Praegu saab programmis:
- lisada ja eemaldada roboteid;
- valida roboti nimekirjast;
- teha tavalist või naljakat tegevust;
- laadida akut ja skaneerida;
- vaadata roboti energiat ja tegevuste logi;
- otsida, sorteerida ja grupeerida roboteid;
- muuta roboti rühma;
- teha järjestikuseid ja paralleelseid arvutusi;
- salvestada ja laadida robotite maailma JSON-, XML- ja LocalDB-vormingus.

## Projekti ülesehitus

Minu projektis on kaks osa:

- **RobotPlanet.Core** – siin on robotite klassid, nende tegevused, andmemudelid ja salvestamise loogika.
- **RobotPlanet.WpfApp** – siin on programmi aken, ViewModel ja kasutajaliides.

Core ei sõltu WPF-ist.

Kasutan MVVM-mustrit, et hoida programmi loogika ja kasutajaliides eraldi.

Robotite hoidmiseks kasutan ka `Repository<T>` klasse.

## Robotid

Mul on üks abstraktne klass `Robot` ja seitse robotit:

- **CleanerBot** – koristab.
- **ExplorerBot** – uurib planeeti.
- **RepairBot** – parandab.
- **DanceBot** – tantsib.
- **ChefBot** – teeb süüa.
- **GuardBot** – valvab.
- **FlyingBot** – lendab.

Igal robotil on nimi, energia, unikaalne ID ja rühm.

Alguses on robotil 100 energiat ja tema rühm on `Default`.

Roboti nime ei saa pärast loomist muuta. Energiat saab muuta roboti meetodite kaudu.

## Liidesed ja tegevused

Kasutasin liideseid:

- `IChargeable` – aku laadimiseks.
- `IScannable` – skaneerimiseks.

Programmis kontrollin `is` abil, mida valitud robot oskab teha.

Igal robotil on kaks peamist meetodit:

- `NormalAction()` – tavaline tegevus.
- `CrazyAction()` – naljakas tegevus.

Robotid teevad neid tegevusi erinevalt.

Tegevused kasutavad energiat. Kui energiat ei ole piisavalt, näitab programm veateadet.

Aku laadimisel ei saa energia ületada 100.

## MVVM ja Repository

Kasutan `MainViewModel` klassi, mis ühendab kasutajaliidese ja programmi loogika.

Robotite hoidmiseks kasutatakse `Repository<T>` klasse.

Kui robot lisatakse, eemaldatakse või teeb tegevuse, saavad teised robotid sellele reageerida.

Reaktsioonid kuvatakse tegevuste logis.

Pärast salvestatud maailma laadimist luuakse õiged robotite tüübid ja taastatakse nende reaktsioonid.

## Programmi aken

Programmis on kolm vahekaarti:

**Robotid**

Siin saab lisada, valida ja eemaldada roboteid, muuta rühma ning kasutada robotite tegevusi. Roboti energiat näitab `ProgressBar`.

**Salvestamine**

Siin saab salvestada ja laadida maailma JSON-, XML- ja LocalDB-vormingus. Samal lehel on ka tegevuste ajalugu.

**Arvutused**

Siin saab valida roboteid ja võrrelda järjestikuseid ning paralleelseid arvutusi.

Kasutajaliidese tekstide jaoks kasutatakse ka `Resources.resx` faili.

## Maailma salvestamine

Lisasin võimaluse salvestada robotite maailma, et andmed ei kaoks pärast programmi sulgemist.

Iga roboti kohta salvestatakse:

- `Id` – unikaalne GUID;
- `Type` – roboti tüüp;
- `Name` – roboti nimi;
- `Energy` – roboti energia;
- `GroupName` – roboti rühm.

Kasutan `RobotDto` ja `WorldDto` klasse.

`WorldMapper` muudab robotid DTO-deks ja loob laadimisel DTO-de põhjal uuesti õiged robotid.

Programmi aknaid, käske ja sündmuste tellimusi ei salvestata.

Kõik kolm salvestusviisi kasutavad sama `IWorldStorage` liidest.

## JSON ja XML

JSON-i jaoks kasutan `JsonWorldStorage` klassi.

XML-i jaoks kasutan `XmlWorldStorage` klassi.

Mõlemad salvestavad sama maailma oleku.

Salvestamiseks ja laadimiseks on programmis järgmised nupud:

- `Salvesta JSON`
- `Laadi JSON`
- `Salvesta XML`
- `Laadi XML`

Faili asukoha saab kasutaja ise valida.

Kui fail on vigane, näitab programm veateadet. Olemasolev maailm jääb alles.

## LocalDB ja ADO.NET

Lisasin ka andmebaasi salvestamise võimaluse.

Kasutan SQL Server LocalDB-d ja `Microsoft.Data.SqlClient` teeki.

### Mida on vaja?

Programmi käivitamiseks on vaja:

- Windowsi;
- Visual Studiot;
- .NET 10 SDK-d;
- SQL Server Express LocalDB-d.

### LocalDB käivitamine

LocalDB olemasolu saab kontrollida käsureal:

```cmd
sqllocaldb info
```

Vajadusel saab LocalDB käivitada:

```cmd
sqllocaldb start MSSQLLocalDB
```

### Ühenduse seadistus

Ühenduse seadistus asub failis `LocalDbWorldStorage.cs`.

Kasutan järgmist ühendust:

```text
Server=(localdb)\MSSQLLocalDB;Database=RobotPlanetDb;Integrated Security=True;TrustServerCertificate=True;
```

Ühendus kasutab Windowsi autentimist.

### Andmebaasi ja tabelite loomine

Programmi esimesel LocalDB kasutamisel luuakse vajadusel automaatselt andmebaas `RobotPlanetDb`.

Andmebaasis on kaks tabelit.

**RobotGroups**
- `GroupId` – primaarvõti;
- `GroupName` – rühma nimi.

**Robots**
- `Id` – roboti GUID ja primaarvõti;
- `RobotType` – roboti tüüp;
- `RobotName` – roboti nimi;
- `Energy` – roboti energia;
- `GroupId` – välisvõti tabelisse `RobotGroups`.

Tabelid on seotud `GroupId` abil.

Andmete salvestamiseks kasutan parameetritega SQL-päringuid.

Kasutan ka tehingut (`transaction`), et robotite ja rühmade andmed salvestataks koos.

### LocalDB kasutamine

1. Käivita Robot Planet.
2. Lisa robot või muuda tema energiat või rühma.
3. Ava vahekaart `Salvestamine`.
4. Vajuta `Salvesta LocalDB`.
5. Sulge programm.
6. Käivita programm uuesti.
7. Vajuta `Laadi LocalDB`.

Pärast laadimist taastatakse robotite andmed.

## Testimine

Kontrollisin programmis järgmisi asju:

1. Roboti lisamine ja eemaldamine töötab.
2. Roboti tavaline ja naljakas tegevus töötab.
3. Energia muutub pärast tegevust.
4. Aku laadimine ja skaneerimine töötavad sobivate robotitega.
5. Tühja nimega robotit ei saa lisada.
6. Roboteid saab otsida, sorteerida ja grupeerida.
7. Roboti rühma saab muuta.
8. JSON-faili salvestamine ja laadimine töötab.
9. XML-faili salvestamine ja laadimine töötab.
10. LocalDB salvestamine ja laadimine töötab.
11. Pärast programmi sulgemist ja uuesti käivitamist taastuvad robotite andmed.
12. Robotite ID-d jäävad pärast JSON-faili laadimist samaks.
13. Roboti energia ja rühm säilivad.
14. Vigase JSON-faili korral kuvatakse veateade ja eelmine maailm jääb alles.
15. Pärast laadimist töötavad robotite tegevused ja reaktsioonid.

## Kuidas programmi käivitada?

1. Ava `RobotPlanet.slnx` Visual Studios.
2. Oota, kuni NuGeti paketid on taastatud.
3. Vali `RobotPlanet.WpfApp` käivitusprojektiks.
4. Vajuta **F5**.

Kasutasin .NET 10 ja WPF-i.

LocalDB kasutamiseks peab arvutis olema SQL Server Express LocalDB.

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
