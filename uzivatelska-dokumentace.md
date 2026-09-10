# GameOfLife — uživatelská dokumentace

Jak program spustit, ovládat a číst jeho výstupy. Nepředpokládá znalost programování.

## 1. Co program dělá

Program simuluje jednoduchý ekosystém na čtvercové ploše 50 × 50 políček. Žijí v něm tři druhy:

- **Producenti** (rostliny, zelené políčko, ikona rostliny) — nehýbou se, energii berou ze slunce a z živin v půdě.
- **Býložravci** (žluté políčko) — pohybují se a jedí sousední producenty.
- **Predátoři** (červené políčko) — pohybují se a jedí sousední býložravce.

Každý tvor má zásobu energie a věk. Když energie dojde nebo tvor zestárne, umře a jeho tělo se stane **živinami** v políčku (odstíny podle množství), které postupně mizí a které může vstřebat rostlina, jež na místě vyroste. Když má tvor energie dost (alespoň 80 % svého maxima), rozmnoží se do volného sousedního políčka; zvířata k tomu navíc potřebují být dospělá a mít vedle sebe partnera stejného druhu.

Býložravci a predátoři se rozhodují, kam jít, pomocí malé „umělé inteligence“ zděděné po rodičích s drobnými odchylkami. Během delšího běhu se tedy může chování populace měnit.

Simulace běží v krocích; jeden krok = každý tvor jednou zestárne, zaplatí energii za život a jedná.

## 2. Spuštění

### 2.1 Požadavky
- [DOPLNIT: .NET SDK / runtime verze], Windows / Linux / macOS.
- Moderní webový prohlížeč [DOPLNIT: testováno v …].

### 2.2 Postup
1. Otevřete příkazový řádek ve složce projektu.
2. Zadejte `dotnet run`.
3. Počkejte na řádek `Now listening on: http://localhost:[DOPLNIT]`.
4. Tuto adresu otevřete v prohlížeči. Simulace se načte rovnou s výchozí populací; zatím neběží.

Ukončení: zavřete prohlížeč a v příkazovém řádku stiskněte `Ctrl+C`.

Pozor: pokud otevřete stránku ve dvou prohlížečích najednou, sdílejí spolu nastavení životních parametrů a seedu. Pro vlastní pokusy používejte jednu záložku.

## 3. Obrazovka

![Popis obrazovky](img/screenshot-annotated.png)

Vlevo je **mřížka** (v záhlaví číslo aktuálního kroku), vpravo **ovládací panel** shora dolů:

1. řádek ikon — Start, Stop, Další krok, Reset, Celá obrazovka
2. **Inicializace** — složení populace při Resetu
3. **Průběžné zásahy** — vlivy, které lze měnit i za běhu
4. řádek s aktuálními počty organismů
5. **legenda grafu** — přepínače jednotlivých křivek a lineární/logaritmické měřítko
6. **graf** vývoje v čase

## 4. Ovládání běhu

| Tlačítko | Co dělá |
|---|---|
| ▶ Start | spustí automatické krokování rychlostí podle posuvníku *Rychlost* |
| ■ Stop | pozastaví; stav zůstane zachován |
| ⏭ Další krok | provede přesně jeden krok (jen když simulace neběží) |
| ↻ Reset | zastaví, vytvoří **novou** populaci podle sekce Inicializace a seedu; historie grafu se smaže |
| ⛶ Celá obrazovka | přepne prohlížeč do celoobrazovkového režimu |

## 5. Nastavení

### 5.1 Inicializace (projeví se až po Resetu)

| Prvek | Význam | Výchozí |
|---|---|---|
| Producenti / Býložravci / Predátoři | pravděpodobnost, že se v každém políčku na začátku objeví daný druh (0–1, tj. 0–100 %). Součet nesmí přesáhnout 1. Například 0,3 = zhruba 30 % políček, tedy asi 750 rostlin. | 0,30 / 0,03 / 0,01 |
| Seed | celé číslo, které určuje „náhodu“. **Stejný seed + stejné nastavení + Reset = stejný průběh.** Změňte číslo, chcete-li jiné rozložení. | 29 |
| *Rovnovážný poměr* | nastaví výchozí pravděpodobnosti a provede Reset | |
| *Náhodný poměr* | vylosuje pravděpodobnosti (celkem nejvýše 60 % zaplnění) a provede Reset. Losování **nezávisí na seedu** — chcete-li běh zopakovat, opište si vylosované hodnoty. | |
| *Přemnožení predátorů* | 0,20 / 0,05 / 0,02 a Reset | |

Poznámka: seed se uplatní až při Resetu. Populace zobrazená hned po načtení stránky je náhodná a nelze ji zopakovat — před pokusem vždy stiskněte Reset.

### 5.2 Průběžné zásahy (projeví se okamžitě, i za běhu)

| Prvek | Význam | Výchozí | Rozsah |
|---|---|---|---|
| Sluneční jas | kolik energie dostávají rostliny každý krok (rostlina získá 40 % této hodnoty). 0 = tma, rostliny přestanou růst. | 200 | 0–200 |
| Rychlost | kroků za sekundu při Startu | 5 | 1–15 |
| Efektivita predace | jaký podíl energie oběti získá ten, kdo ji sní | 0,3 | 0–1 |
| Náklady na reprodukci | podíl maxima energie, který rodič zaplatí za potomka (u zvířat oba rodiče) | 0,3 | 0–1 |
| Náklady na pohyb | podíl maxima energie za jeden přesun zvířete | 0,0002 | 0–1 |
| Bazální metabolismus | podíl maxima energie, který každý tvor zaplatí každý krok | 0,0001 | 0–1 |
| *Výchozí životní parametry* | vrátí čtyři hodnoty výše na výchozí | | |
| *Náhodné životní parametry* | každou z nich vynásobí náhodným číslem 0,5–2 (nezávisle na seedu; hodnoty se ukážou v polích) | | |

Do polí zadávejte desetinnou tečku [DOPLNIT: ověřit chování s čárkou v prohlížeči].

## 6. Jak číst výstupy

### 6.1 Mřížka

| Vzhled políčka | Význam |
|---|---|
| tmavé | prázdno |
| zelené s ikonou | producent |
| žluté s ikonou | býložravec |
| červené s ikonou | predátor |
| [DOPLNIT barvy] tři odstíny bez ikony | živiny — mnoho (> 20), střední (5–20), málo (< 5) |

Tvor, který byl v tomto kroku sežrán, může být na políčku vidět ještě do dalšího kroku.

### 6.2 Graf
Vodorovná osa = krok (od 0 do aktuálního kroku, graf se průběžně „smršťuje“), svislá osa = hodnota. Číslo vpravo dole je aktuální krok.

| Křivka | Barva | Co ukazuje | Měřítko |
|---|---|---|---|
| Producenti / Býložravci / Predátoři | zelená / žlutá / červená | počet jedinců | společné: maximum ze všech tří |
| Energie | tyrkysová | celková energie všech tvorů a živin | maximum celkové energie |
| Sluneční jas | bílá | 0–1 (podíl z 200) | pevné 0–1 |
| Průměrná energie producentů / býložravců / predátorů | tmavě zelená / oranžová / tmavě červená | energie na jednoho jedince | stejné jako celková energie (proto jsou tyto křivky nízko) |

Tlačítko **Scale: log/lin** přepíná logaritmické měřítko (výchozí; zvýrazní malé populace vedle velkých) a lineární. Tlačítka legendy jednotlivé křivky skrývají.

Typické průběhy:
- **Vlny** býložravců následované vlnami predátorů s malým zpožděním — dravci „dohánějí“ kořist (model dravec–kořist).
- **Vymření predátorů** — nejčastější konec: dravci neseženou dost býložravců, zestárnou a vymřou; býložravci a rostliny pak oscilují dál.
- **Kolaps rostlin** — při vysoké efektivitě predace nebo mnoha býložravcích; po něm vymřou i býložravci.
- **Propad energie** po snížení slunce — rostliny přestanou doplňovat energii systému.

### 6.3 Výstupní soubory
Program nic neukládá ani neexportuje. Pro záznam pokusu použijte screenshot a opište hodnoty nastavení.

## 7. Časté problémy

| Problém | Řešení |
|---|---|
| Stránka se nenačte | ověřte, že příkazový řádek s `dotnet run` stále běží a adresa/port souhlasí |
| Tlačítka Start / Další krok jsou šedá | Start je nedostupný, když simulace běží; Další krok je nedostupný za běhu — stiskněte Stop |
| Simulace je trhaná | snižte rychlost; překreslení 2 500 políček každý krok je náročné, zvlášť na slabším zařízení |
| Všechno vymře během pár desítek kroků | vraťte *Výchozí životní parametry* a *Rovnovážný poměr*; zkontrolujte, že sluneční jas není u nuly |
| Po změně Producenti/Býložravci/Predátoři se nic nestalo | tato nastavení platí až po Resetu |
| Stejný seed dává jiný výsledek | seed platí až po Resetu; presety *Náhodný…* losují mimo seed; a zkontrolujte, zda nebyla stránka otevřená ve více záložkách |

## 8. Omezení
- Pevná velikost 50 × 50; nelze měnit z rozhraní.
- Jedna simulace na server; více otevřených záložek si zasahuje do nastavení.
- Bez ukládání, načítání a exportu.
- Historie grafu roste s délkou běhu; velmi dlouhé běhy (desítky tisíc kroků) zpomalují překreslování.
