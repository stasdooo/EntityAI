# AI knihovna pro cílově řízené chování entit v Unity

## Motivace a účel

Ve hrách se tradičně chování NPC (non-player characters) řeší ručním skriptováním: "pokud je hladový, udělej X", "pokud má hlad, ale je blízko zdroje, udělej Y"… Tento přístup je těžko udržitelný, když postavy mají více potřeb a svět je složitější.

Tato knihovna přináší modulární a přenositelný způsob, jak místo ručního skriptování popsat cíle a stav světa. AI systém pak automaticky generuje chování NPC – rozhoduje, které akce provést a v jakém pořadí, aby naplnily své cíle.

Knihovna je inspirována principy GOAP (Goal-Oriented Action Planning), ale je koncipována jednodušeji a přehledněji. Je snadno integrovatelná i do menších projektů, a funguje jak samostatně v konzoli, tak v Unity (s adaptérem).

## Logování

Pro sledování chování je možné do simulace vložit logger implementující rozhraní [`IAiLogger`](src/GoalAI.Core/Diagnostics/IAiLogger.cs)
(Rozhraní lze rozšířit o podrobnější výpisy)

## Základní principy

### Entity

Každá postava nebo objekt s chováním je **entita (Entity)**.
Entita se skládá z **komponent**

### Komponenty

Každá komponenta implementuje rozhraní [`IComponent`](src/GoalAI.Core/Components/IComponent.cs). Některé navíc [`ITickable`](src/GoalAI.Core/ITickable.cs) – to znamená, že se mění v čase. Stav se aktualizuje při každém taktu simulace (např. `HungerComponent` zvyšuje hlad)

**Podporované [komponenty](src/GoalAI.Core/Components):**

- **HungerComponent** – reprezentuje hlad (0–100). Každým tikem se zvyšuje (ITickable).
- **InventoryComponent** – udržuje stav inventáře (v základní verzi pouze počet jídla).
- **PositionComponent** – uchovává aktuální souřadnice entity ve světě a umožňuje výpočty vzdáleností.
- **EntityBlackboardComponent** – lokální "blackboard", sdílený kontejner hodnot mezi akcemi a cíli (např. FoodTarget).
- **PropertiesComponent** – klíč/hodnota pro číselné vlastnosti.
- **TagsComponent** – jednoduchá množina textových značek.
- **AIComponent** – speciální komponenta, která registruje dostupné akce a cíle a propojuje entitu s plánovačem.

### Akce

**[`Akce`](src/GoalAI.Core/Actions) ([`IAction`](src/GoalAI.Core/IAction.cs))** popisují, co entita může vykonat.

**Podporované:**

- **EatAction** – sní jídlo z inventáře, sníží hlad.
- **MoveToAction** – přesune entitu na cílovou pozici uloženou v blackboardu.
- **PickUpAction** – sebere jídlo z resource node, pokud stojí na správné pozici.
- **MakeFoodAction** – "uměle" vytvoří jídlo (je drahá a má dlouhý cooldown).

**Rozšíření akcí:**

- **IDurationAction** – akce trvá v čase (např. MoveToAction podle vzdálenosti).
- **ICooldownAction** – akce má cooldown (např. MakeFoodAction nebo EatAction).

### Cíle

**[`Cíle`](src/GoalAI.Core/Goals) ([`IGoal`](src/GoalAI.Core/IGoal.cs))** popisují žádoucí stavy.
Každý cíl má prioritu a test splnění.

**Podporované:**

- **AvoidHungerGoal** – snižuj hlad pod určitou hranici.
- **StayAliveGoal** – nepřekroč hladinu „maximálního hladu“.
- **CollectFoodGoal** – mít v inventáři určitý minimální počet jídla.

### Svět

- **[`World`](src/GoalAI.Core/World.cs)** – obsahuje entity a zdroje.
- **[ResourceRegistry](src/GoalAI.Core/Resources)** – kolekce všech zdrojů (např. hromádky jídla).
- **[ResourceNode](src/GoalAI.Core/Resources)** – konkrétní zdroj, definovaný typem, pozicí a množstvím.
- **[ResourceType](src/GoalAI.Core/Resources)** – enum type (aktuálně FoodPile, do budoucna lze rozšířit o další zdroje (dřevo, kámen atd.)).

 ### Simulation

Třída **[`Simulation`](src/GoalAI.Core/Simulation.cs)** spravuje běh světa:

- Tickuje všechny `ITickable` komponenty.
- Sleduje probíhající akce, jejich časování a cooldowny.
- Pravidelně spouští plánovač a hledá nové plány pro nesplněné cíle.
- Přiřazuje a spouští akce.

### GOAP Planner

**[`GoapPlanner`](src/GoalAI.Core/Planning/GoapPlanner.cs)** hledá posloupnost plánovacích akcí (`IPlanningAction`), která vede k dosažení cíle. Implementuje jednoduchý plánovač ve stylu GOAP s prohledáváním prostoru stavů (BFS nebo Dijkstra podle ceny akcí)
 Pokud se podaří dosáhnout cílového stavu (podle predikátu), vrací posloupnost plánovacích akcí
 
Rozhraní `IPlanningAction` definuje plánovací akci:

- **`CheckPreconditions(PlanState)`** – testuje, zda lze akci provést v daném stavu
- **`ApplyEffects(PlanState)`** – aplikuje efekty akce na stav (jak se změní stav po provedení)
- **`Cost(PlanState)`** – vypočítá cenu akce v daném stavu
- **`RuntimeAction`** – odkaz na skutečnou runtime akci, která se spustí při exekuci

#### PlanState - abstraktní reprezentace světa

[`PlanState`](src/GoalAI.Core/Planning/PlanState.cs) je abstraktní reprezentace světa, se kterou pracuje plánovač:

- Mapa klíč → hodnota (int) - např. `food=2`, `hunger=70`, `at_target=1`
-  Podporuje klonování (`Clone`) a porovnání (`Equals`, `GetHashCode`), aby bylo možné stav ukládat do `HashSet`u a kontrolovat duplicity
-  **Důležité:**  
Pokud klient rozšiřuje knihovnu a přidává nové komponenty nebo vlastnosti, je **nutné rozšířit metodu `BuildPlanState(Entity e)`** a zapsat relevantní hodnoty do `PlanState`. Tato metoda slouží jako most mezi konkrétními komponentami entity a abstraktní reprezentací stavu používanou plánovačem.

## Typický agent

Výchozí agent (např. sedlák/peasant):

- Sleduje cíl: nenechat hlad vyrůst příliš vysoko (`AvoidHungerGoal`)
- Plánuje akce: přesun ke zdroji → sebrání jídla → jídlo do inventáře → snězení
- Fallback: pokud nejsou zdroje poblíž, použije `MakeFoodAction`
