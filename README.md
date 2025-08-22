# AI knihovna pro cílově řízené chování entit v Unity

## Motivace a účel
V mnoha hrách se chování postav (NPC) programuje ručně pomocí skriptů, což je neudržitelné při větší komplexitě. Cílem této knihovny je nabídnout modulární, přenositelný a deklarativní způsob tvorby chování entit založený na cílech a aktuálním stavu herního světa.

Inspirace čerpám z principů jako GOAP (Goal-Oriented Action Planning), ale cílem je vytvořit jednodušší, přehlednější a snadno integrovatelnou knihovnu, vhodnou i pro menší projekty.

## Popis fungování
Knihovna bude použitelná jak v Unity, tak samostatně (např. pro testování). Chování postav bude definováno pomocí:

- **Entit** - skládají se z komponent (např. `HungerComponent`, `InventoryComponent`, `AIComponent`)
- **Akce** - které postava může provádět (např. `EatAction`, `ForageAction`)
- **Cílů** - kterých se má snažit dosáhnout (např. `AvoidHunger`, `StayAlive`)



AI systém sám určuje, které akce provede, a v jakém pořadí, aby dosáhla nejdůležitějšího cíle.
Simulace probíhá v čase, během každého kroku se aktualizuje stav světa i rozhodování.

## Ukázka použití API
```csharp
var entity = new Entity("Peasant");
entity.AddComponent(new HungerComponent());
entity.AddComponent(new InventoryComponent());
entity.AddComponent(new AIComponent());

entity.AI.AddAction(new ForageAction());
entity.AI.AddGoal(new Goal("AvoidHunger", priority: 5));

world.AddEntity(entity);
Simulation.Run(world);
```
## Komunikace s okolním světem
Knihovna poskytne:

- **Veřejné API** (`Entity`, `Component`, `Action`, `Goal`, `Simulation`)
- **Možnost konfigurace přes C#** (žádné skriptování v Unity editoru)
- **Volitelnou Unity adaptaci** (např. `AIController` jako `MonoBehaviour`), která propojí knihovnu se scénou

## Formát vstupů a výstupů
- **Vstup**: konfigurace entit a světa pomocí C# API
- **Výstup**:
  - Chování entit v simulaci
  - Ladicí výstupy (např. `DebugLog`)
  - Vizualizace v Unity

## GUI a způsob spouštění
- **Unity projekt** - hlavní prostředí pro:
  - Spuštění simulace
  - Vizualizaci chování entit
- **Konzolová aplikace** - alternativní způsob spouštění (např. pro testování bez Unity)
- **Základní GUI v Unity scéně** zobrazující:
  - Stav světa
  - Entity
  - Jejich plánované akce

## Použité technologie

 C# / .NET, Unity


## Budoucí rozšíření
- Editorové nástroje pro snadnější konfiguraci
- Optimalizace pro větší množství entit
- Podpora více agentů a jejich interakcí
