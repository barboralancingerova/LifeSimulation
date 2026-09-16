# GameOfLife — simulace ekosystému s evolucí

> Zápočtový program, NPRG031, letní semestr 2025/26
> Autor: Barbora Lancingerová
> Technologie: C# / .NET 8.0, Blazor Server (InteractiveServer), bez externích knihoven

(main.jpg)

## Anotace

Program simuluje jednoduchý ekosystém na mřížce 50×50. Žijí v něm tři druhy: producenti
(rostliny, získávají energii ze slunce a z živin v půdě), býložravci (jedí producenty)
a predátoři (jedí býložravce). Každý organismus má energii, věk a genom u zvířat
genom obsahuje i váhy malé neuronové sítě (33 → 16 → 8), která rozhoduje o směru pohybu.
Při rozmnožování se genomy rodičů průměrují a mutují, takže se chování populací může
v průběhu běhu měnit. Odumřelé organismy se mění na živiny, které producenti vstřebávají.

Uživatel simulaci ovládá ve webovém prohlížeči: spouští, krokuje, mění za běhu
intenzitu slunce a energetické náklady, sleduje populační křivky a energii systému
v grafu. Nastavením seedu lze běh zopakovat.

## Co program dělá — pravidla ve zkratce

- Mřížka 50×50 s pevnými okraji (není toroidní). V buňce je nejvýše jeden organismus a případně živiny.
- Krok simulace: projdou se všechny buňky, každý organismus zaplatí metabolismus a zestárne, pak jedná.
- Producent: fotosyntéza (`0,4 × sluneční jas` energie za krok), vstřebání živin z vlastní buňky, při energii ≥ 80 % maxima se rozmnoží do volné buňky v okolí 5×5.
- Zvíře (býložravec / predátor): vidí okolí 9×9. Reflexy — pokud je vedle něj potrava, sní ji; pokud je dospělé, má ≥ 80 % energie a vedle něj je vhodný partner, rozmnoží se. Pokud nejedlo, pohne se ve směru, který vybere neuronová síť.
- Smrt: energie ≤ 0 nebo věk > maximální věk. 30 % energie mrtvého těla se stane živinami v buňce; živiny se každý krok zmenšují o 5 %.
- Dědičnost: geny (max. věk, max. energie, …) a váhy NN potomka = průměr rodičů + gaussovský šum (σ = 0,05), s omezením do povolených mezí.

Podrobná pravidla, energetický model a struktura kódu jsou v
[programátorské dokumentaci](docs/programatorska-dokumentace.md); ovládání a čtení výstupů
v [uživatelské dokumentaci](docs/uzivatelska-dokumentace.md).

## Rychlý start

```bash
git clone https://github.com/barboralancingerova/LifeSimulation.git
cd GameOfLife
dotnet run
```

Aplikace poběží na `http://localhost:5294`; simulace je na kořenové stránce `/`.
Požadavek: .NET SDK 8.0.

## Struktura repozitáře

```
GameOfLife/
├── README.md
├── docs/
│   ├── programatorska-dokumentace.md
│   ├── uzivatelska-dokumentace.md
│   ├── experimenty/          # protokoly běhů
│   └── img/                  # screenshoty
├── Components/
│       ├── Simulation        # Config, Energy, Cell, Grid, Organism, Producer,
│       │                     # Herbivore, Predator, Genome, NeuralNetwork,
│       │                     # Statistics, RuntimeSettings
│       └── Pages             # SimulationPage.razor (+css), Chart.razor (+css),
│                             # InitControls.razor, RuntimeControls.razor (nepoužité)
├── wwwroot/img/              # ikony organismů a tlačítek
└── Program.cs                # spuštění a obsluha serveru
```

## Stav projektu

Hotovo: simulační jádro, evoluce genomu a NN, Blazor GUI s ovládáním, grafy a presety.
Známé nedostatky (detail v programátorské dokumentaci, kap. 8): sekvenční průchod mřížky
zvýhodňuje pohyb „dopředu“; absolutní σ mutace je pro velké geny prakticky nulová; globální
statický stav (`Grid.Rng`, `RuntimeSettings`) sdílený mezi všemi uživateli serveru.


