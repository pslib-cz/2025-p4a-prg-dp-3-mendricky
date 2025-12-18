# MULTIDIMENZIONÁLNÍ ABSŤÁK (MDA) 🌌

Konzolová RPG hra simulující cestu hrdiny multivesmírem při hledání bájného Nápoje Euforie.

## 🎮 Herní funkce

### Statistiky hráče
- **❤️ Zdraví (HP)** - Klesne na 0 = Game Over
- **🧠 Příčetnost (Sanity)** - Klesne na 0 = Šílenství (také Game Over)
- **💰 Kredity** - Měna pro nákup předmětů
- **⚔️ Útok / 🛡️ Obrana** - Ovlivňují boj

### Bojový systém
- **Útok** - Způsobí poškození nepříteli
- **Obrana** - Sníží příští obdržené poškození
- **Použít předmět** - Využij věci z inventáře
- **Útěk** - 33% šance na únik (100% s Portálovou tekutinou)

### Předměty
| Předmět | Efekt |
|---------|-------|
| Léčivý lektvar | +30 HP |
| Prášek na klid | +25 Sanity |
| Mega Steroid | +10 Attack (permanentní) |
| Portálová tekutina | Garantovaný útěk |
| Záhadné maso | +40 HP nebo -10 HP a -5 Sanity |
| Čistič na Plumbus | +10 Sanity |
| 🌟 Bájný nápoj euforie 🌟 | **CÍL HRY** |

### Lokace (5 dimenzí)
1. **Dimenze zbytečných emocí** - Vysoké poškození Sanity
2. **Stanice 'U Poslední Naděje'** - Neonové bary a dealery
3. **Dimenze nekonečného papírování** - Byrokratické peklo
4. **Svět Cronenbergů** - Masité horory
5. **Planeta Jednoty** - Kolektivní vědomí

### Boss systém
Každých **5 lokací** se objeví BOSS - silnější nepřítel s šancí upustit Nápoj Euforie!

---

## 🏗️ Použitý návrhový vzor: Factory Method (Tovární metoda)

V této aplikaci je použit návrhový vzor **Factory Method**.

### Proč?
Tento vzor umožňuje definovat rozhraní pro vytváření objektů, ale o konkretizaci rozhodují až podtřídy. V našem případě každá lokace (`ILocation`) funguje jako "továrna" na své vlastní specifické nepřátele (`IEnemy`).

**Hlavní výhody:**
1. **Low Coupling (Nízká závislost):** Hlavní herní smyčka v `GameEngine` vůbec neví o existenci konkrétních tříd jako `NebulaOfSadness` nebo `CyborgDealer`. Pracuje pouze s rozhraními `ILocation` a `IEnemy`.
2. **Extensibility (Rozšiřitelnost):** Pokud chceme přidat novou lokaci s novými monstry, stačí vytvořit nové třídy a zaregistrovat je v `Program.cs`. Kód v `GameEngine` zůstává beze změny.
3. **Open/Closed Principle:** Kód je otevřený pro rozšíření (nový obsah), ale uzavřený pro modifikaci (nemusíme sahat do jádra hry).

### Diagram architektury

```
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│  GameEngine │──────▶│  ILocation  │──────▶│   IEnemy    │
└─────────────┘       └─────────────┘       └─────────────┘
                            ▲                     ▲
         ┌──────────────────┼──────────────────┐  │
         │                  │                  │  │
┌────────┴────────┐ ┌───────┴───────┐ ┌───────┴───────┐
│ NebulaOfSadness │ │ NeonStation   │ │ Cronenberg... │
├─────────────────┤ ├───────────────┤ ├───────────────┤
│ SpawnEnemy()    │ │ SpawnEnemy()  │ │ SpawnEnemy()  │
│ SpawnBoss()     │ │ SpawnBoss()   │ │ SpawnBoss()   │
└─────────────────┘ └───────────────┘ └───────────────┘
         │                  │                  │
         ▼                  ▼                  ▼
┌─────────────────┐ ┌───────────────┐ ┌───────────────┐
│  VoidSpecter    │ │ CyborgDealer  │ │CronenbergAlpha│
│  SadClown       │ │ DrunkPirate   │ │  FleshBlob    │
│ DepressionBoss  │ │ NeonOverlord  │ │CronenbergMonst│
└─────────────────┘ └───────────────┘ └───────────────┘
```

---

## 🚀 Jak přidat nový obsah?

### Přidání nové lokace s nepřáteli:

1. Vytvořte nový soubor v `/Locations/` (např. `MojaNovaLokace.cs`)

2. Implementujte nepřátele pomocí `BaseEnemy`:
```csharp
public class MujNepřítel : BaseEnemy
{
    public override string Name => "Název";
    public override int MaxHealth => 50;
    public override int Attack => 15;
    public override int Defense => 5;
    public override int RewardCredits => 30;
    public override int SanityDamage => 10;

    public override string GetEncounterText() => "Text při setkání";
    public override string GetAttackText() => "Text při útoku";
    public override string GetDeathText() => "Text při smrti";
}
```

3. Implementujte lokaci:
```csharp
public class MojeNovaLokace : ILocation
{
    public string Name => "Název lokace";
    public string Description => "Popis lokace";
    public ConsoleColor ThemeColor => ConsoleColor.Green;
    public bool HasBoss => true;
    public string[] AsciiArt => new[] { "ASCII obrázek" };

    public IEnemy SpawnEnemy() => new MujNepřítel();
    public IEnemy SpawnBoss() => new MujBoss();
    public string GetEventText() => "Náhodná událost";
    public IItem? GetLocationItem() => null;
}
```

4. Zaregistrujte v `Program.cs`:
```csharp
engine.RegisterLocation(new MojeNovaLokace());
```

---

## 🎯 Jak vyhrát?

1. Přežij boje (udržuj HP nad 0)
2. Zachovej si příčetnost (Sanity nad 0)
3. Poraž BOSSE
4. Získej **Bájný nápoj euforie**
5. ???
6. Profit (možná)

---

*Tento projekt byl vytvořen jako součást cvičení na DP (Design Patterns).*

**Verze:** Existenciální krize 2.0 🌀
