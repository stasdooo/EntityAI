# Unity adaptér – poznámky k použití

**Poznámka:**  
Dané soubory jsou pouze nástin, jak by šlo knihovnu používat.  
Uživatel se musí umět orientovat v základech Unity (Hierarchy, Inspector, komponenty).

---

## Přehled souborů

### `AiWorldBootstrap.cs`
- **Účel:** Inicializuje globální `World` a `Simulation`. Drží je jako singleton (`Instance`).
- **Jak použít:**  
  1. Vytvoř v **Hierarchy** prázdný objekt `GoalAI`.  
  2. Přidej na něj komponentu **`AiWorldBootstrap`**.  
  3. Nastav v Inspectoru např. `replanInterval = 0.25`.

---

### `AiSimulationRunner.cs`
- **Účel:** Každý frame volá `Simulation.Step(Time.deltaTime)`, aby svět běžel.  
- **Jak použít:**  
  1. Na objekt `GoalAI` přidej i **`AiSimulationRunner`**.  
  2. Hotovo – simulace se spustí automaticky při Play.

---

### `UnityAiLogger.cs`
- **Účel:** Implementace rozhraní `IAiLogger`. Píše logy o výběru cílů a akcí do Unity Console.  
- **Jak použít:**  
  - Je instanciován v `AiWorldBootstrap` a předán do `Simulation`.  
  - Lze rozšířit o podrobnější výpisy.

---

### `ResourcePileAuthoring.cs`
- **Účel:** Zaregistruje do světa zdroj (`ResourceNode`) podle pozice GameObjectu.  
- **Jak použít:**  
  1. Vytvoř `GameObject => 2D Object => Sprite`(nebo jakýkoliv jiný objekt) 
  2. Nastav `Transform.position` na souřadnice zdroje (např. `5,3,0`).  
  3. Přidej komponentu **`ResourcePileAuthoring`**.  
  4. V Inspectoru nastav `amount` (např. `5`).  
  5. Play → zdroj se přidá do `World.Resources`.

---

### `AiAgentController.cs`
- **Účel:** Převádí GameObject na AI entitu. Vytvoří `Entity`, přidá komponenty (`HungerComponent`, `InventoryComponent`, `PositionComponent`, `EntityBlackboardComponent`), zaregistruje akce a cíle a přidá entitu do `World`.  
- **Jak použít:**  
  1. Vytvoř GameObject, pojmenuj napřiklad `Peasant`.  
  2. Přidej komponentu **`AiAgentController`**.  
  3. V Inspectoru nastav parametry:  
     - `initialHunger`  
     - `hungerPerSecond`  
     - `moveSpeed`  
  4. Play → agent začne plánovat a vykonávat akce.

---

## Minimální scéna

1. **Hierarchy**:
   - `GoalAI` (+ `AiWorldBootstrap`, + `AiSimulationRunner`)
   - `Food A` (+ `ResourcePileAuthoring`, `amount = 5`, pozice např. `5,3,0`)
   - `Food B` (+ `ResourcePileAuthoring`, `amount = 3`, pozice např. `-2,1,0`)
   - `Peasant` (+ `AiAgentController`, parametry dle potřeby)

2. **Play**:
   - V **Console** se zobrazí logy (`UnityAiLogger`).  
   - V **Scene**/Game View uvidíme, jak agent "hledá", "sbírá" a "jí" jídlo.
