using MDA.Core;

namespace MDA.Locations;

public class VoidSpecter : BaseEnemy
{
    public override string Name => "Fyzikální anomálie s depresí";
    public override int MaxHealth => 40;
    public override int Attack => 12;
    public override int Defense => 2;
    public override int RewardCredits => 20;
    public override int SanityDamage => 15;

    public override string GetEncounterText() => "Z mlhoviny se vynořuje cosi, co vypadá jako tvoje nezaplacené účty. Má to chapadla a velmi nízké sebevědomí.";
    public override string GetAttackText() => "Anomálie ti předhazuje všechny tvoje životní selhání. -15 Sanity!";
    public override string GetDeathText() => "Anomálie se rozplývá do nicoty. Stejně jako tvoje sny.";

    public override IItem? GetLoot() => new Random().Next(0, 3) == 0 ? new SanityPill() : null;
}

public class SadClown : BaseEnemy
{
    public override string Name => "Smutný kosmický klaun";
    public override int MaxHealth => 55;
    public override int Attack => 15;
    public override int Defense => 3;
    public override int RewardCredits => 30;
    public override int SanityDamage => 20;

    public override string GetEncounterText() => "Objevuje se klaun s nekonečnými slzami válícími se po obličeji. Jeho rudý nos bliká jako maják beznaděje.";
    public override string GetAttackText() => "Klaun ti ukazuje balónky ve tvaru tvých zklamaných rodičů!";
    public override string GetDeathText() => "Klaun se naposledy usmívá. Poprvé za tisíc let. A pak je pryč.";
}

public class DepressionBoss : BaseEnemy
{
    public override string Name => "VELKÁ PRÁZDNÁ NICOTA";
    public override int MaxHealth => 120;
    public override int Attack => 25;
    public override int Defense => 8;
    public override int RewardCredits => 100;
    public override int SanityDamage => 30;

    public override string GetEncounterText() => "Celá dimenze ztichne. Před tebou stojí... NIC. A přesto to NIC má oči a dívá se přímo na tebe.";
    public override string GetAttackText() => "Nicota tě objímá. Zasahuje tě tam, kde to bolí nejvíc - v duši.";
    public override string GetDeathText() => "Nicota se zaplňuje tvým vzdorem. Někde v dáli něco slabě zajásá.";

    public override IItem? GetLoot() => new Random().Next(0, 4) == 0 ? new EuphoriaPotion() : new AttackBoost();
}

public class NebulaOfSadness : ILocation
{
    private static readonly Random _rng = new();

    public string Name => "Dimenze zbytečných emocí";
    public string Description => "Tuhle dimenzi vytvořil nějakej ufňukanej bůh, kterej neměl na Netflix. Všichni jsou tu smutní a smrdí to tu jako starej sklep.";
    public ConsoleColor ThemeColor => ConsoleColor.DarkBlue;
    public bool HasBoss => true;

    public string[] AsciiArt => new[]
    {
        @"      。  ✧  ・゜  ˚ *  。 ☆  。",
        @"   * . ﾟ °   。  ☆  ˚    ✧",
        @"  ~(   T_T   )~    * ˚  。 ゜",
        @"     ˚  * 。  ☆  ·  ✧  ˚ *",
        @"   ☆ ° . ˚ 。 ゜ ·  。  * ˚"
    };

    public IEnemy SpawnEnemy()
    {
        return _rng.Next(0, 2) == 0 ? new VoidSpecter() : new SadClown();
    }

    public IEnemy SpawnBoss() => new DepressionBoss();

    public string GetEventText()
    {
        string[] events = {
            "Nacházíš starou deníkovou stránku. Je to jen jeden řádek: 'Proč?'",
            "Někdo tu nechal krabici papírových kapesníčků. Polovičních.",
            "Slyšíš vzdálený pláč. Nebo je to smích? Těžko říct.",
            "Nacházíš automat na radost. Je vypnutý. Asi navždy."
        };
        return events[_rng.Next(events.Length)];
    }

    public IItem? GetLocationItem()
    {
        return _rng.Next(0, 4) == 0 ? new SanityPill() : null;
    }
}
