using MDA.Core;

namespace MDA.Locations;

public class CyborgDealer : BaseEnemy
{
    public override string Name => "Předraženej automat na štěstí";
    public override int MaxHealth => 50;
    public override int Attack => 14;
    public override int Defense => 5;
    public override int RewardCredits => 35;
    public override int SanityDamage => 8;

    public override string GetEncounterText() => "Kyber-dealer se ti snaží vnutit 'pohodu v tabletce'. Vypadá to jako paralen s příchutí mentolu. Trapný.";
    public override string GetAttackText() => "Dealer po tobě hází prošlé léky! Jeden tě trefil přímo do oka!";
    public override string GetDeathText() => "Automat jiskří a vybuchuje. Z něj vypadávají mince a podezřelé prášky.";

    public override IItem? GetLoot() => new Random().Next(0, 2) == 0 ? new HealthPotion() : null;
}

public class DrunkSpacePirate : BaseEnemy
{
    public override string Name => "Zpitý vesmírný pirát";
    public override int MaxHealth => 65;
    public override int Attack => 18;
    public override int Defense => 4;
    public override int RewardCredits => 45;
    public override int SanityDamage => 10;

    public override string GetEncounterText() => "Pirát se motá kolem baru a křičí něco o 'pokladu na konci multivesmíru'. Páchne jako destilovaná hvězdná matérie.";
    public override string GetAttackText() => "Pirát po tobě mrští prázdnou lahví! Netrefil, ale jeho dech tě zasáhl naplno!";
    public override string GetDeathText() => "Pirát padá na bar a začíná chrápat. Bitva skončila... nebo prostě usnul.";

    public override IItem? GetLoot() => new MysteryMeat();
}

public class NeonOverlord : BaseEnemy
{
    public override string Name => "NEONOVÝ PAŠERÁK XenonBoss-3000";
    public override int MaxHealth => 100;
    public override int Attack => 22;
    public override int Defense => 7;
    public override int RewardCredits => 120;
    public override int SanityDamage => 15;

    public override string GetEncounterText() => "Z VIP sekce vychází obrovská postava obklopená neonovými světly. 'TY. MOU STANICI. MŮJ BYZNYS. SKONČI.'";
    public override string GetAttackText() => "Boss ti střílí laserové paprsky z očí! Klasika.";
    public override string GetDeathText() => "XenonBoss exploduje v kaskádě neonových barev. Je to vlastně docela hezký... pro smrt.";

    public override IItem? GetLoot() => new Random().Next(0, 5) == 0 ? new EuphoriaPotion() : new AttackBoost();
}

public class NeonSpaceStation : ILocation
{
    private static readonly Random _rng = new();

    public string Name => "Stanice 'U Poslední Naděje' (pobočka B)";
    public string Description => "Místo plné neonů, který ti vypálí sítnici, a bytostí, co mají víc končetin než IQ. Ideální místo pro to se ztratit... nebo se poblejt.";
    public ConsoleColor ThemeColor => ConsoleColor.Magenta;
    public bool HasBoss => true;

    public string[] AsciiArt => new[]
    {
        @"   ╔══════════════════════════╗",
        @"   ║   🌟 NEON STATION 🌟    ║",
        @"   ║  ░▒▓████▓▒░▒▓████▓▒░    ║",
        @"   ║    ╭─────────────╮      ║",
        @"   ║    │ DRINKS 24/7 │      ║",
        @"   ╚══════════════════════════╝"
    };

    public IEnemy SpawnEnemy()
    {
        return _rng.Next(0, 2) == 0 ? new CyborgDealer() : new DrunkSpacePirate();
    }

    public IEnemy SpawnBoss() => new NeonOverlord();

    public string GetEventText()
    {
        string[] events = {
            "Někdo ti nabízí 'zázračný nápoj'. Je to jen voda s glitry.",
            "DJská kabina hraje intergalaktický techno. Bolí tě z toho zuby.",
            "Občerstvení je zdarma! ...ale je to pouze syntetická krev pro vampíry.",
            "Najdeš starou blešku. Koupíš si Plumbus. Proč? Protože můžeš."
        };
        return events[_rng.Next(events.Length)];
    }

    public IItem? GetLocationItem()
    {
        if (_rng.Next(0, 5) == 0) return new HealthPotion();
        if (_rng.Next(0, 3) == 0) return new PlumbusCleaner();
        return null;
    }
}
