# MULTIDIMENZIONÁLNÍ ABSŤÁK (MDA)

Konzolová hra simulující nekonečnou cestu hrdiny vesmírem při hledání bájného nápoje euforie.

## Použitý návrhový vzor: Factory Method (Tovární metoda)

V této aplikaci je použit návrhový vzor **Factory Method**.

### ?
Tento vzor umožňuje definovat rozhraní pro vytváření objektů, ale o konkretizaci rozhodují až podtřídy. V našem případě každá lokace (`ILocation`) funguje jako "továrna" na své vlastní specifické nepřátele (`IEnemy`).

**Hlavní výhody:**
1. **Low Coupling (Nízká závislost):** Hlavní herní smyčka v `GameEngine` vůbec neví o existenci konkrétních tříd jako `NebulaOfSadness` nebo `CyborgDealer`. Pracuje pouze s rozhraními `ILocation` a `IEnemy`.
2. **Extensibility (Rozšiřitelnost):** Pokud chceme přidat novou lokaci (např. "Planeta skladu Amazonu") s novým monstrem ("Unavený skladník"), stačí vytvořit nové třídy a zaregistrovat je v `Program.cs`. Kód v `GameEngine` zůstává beze změny.
3. **Open/Closed Principle:** Kód je otevřený pro rozšíření (nový obsah), ale uzavřený pro modifikaci (nemusíme sahat do jádra hry).

### Jak přidat nový obsah?
1. Vytvořte novou třídu implementující `IEnemy`.
2. Vytvořte novou třídu implementující `ILocation`.
3. V metodě `SpawnEnemy()` nové lokace vraťte instanci nového nepřítele.
4. Zaregistrujte novou lokaci v `Program.cs` pomocí `engine.RegisterLocation(new MojeNovaLokace())`.

---
*Tento projekt byl vytvořen jako součást cvičení na DP (Design Patterns).*
