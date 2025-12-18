using MDA.Core;

namespace MDA.Locations;

public class CronenbergMonster : BaseEnemy
{
    public override string Name => "Cronenbergovský Morty";
    public override int MaxHealth => 55;
    public override int Attack => 20;
    public override int Defense => 3;
    public override int RewardCredits => 35;
    public override int SanityDamage => 20;

    public override string GetEncounterText() => "Vypadá to jako hrouda masa, očí a zubů, co se tě snaží obejmout. Je to odporný. Je to smutný. Je to prostě vesmír.";
    public override string GetAttackText() => "Cronenberg se na tebe vrhá svými chapadlo-končetinami!";
    public override string GetDeathText() => "Masa se roztéká po zemi. Někde uvnitř to pořád bliká. Asi oko.";

    public override IItem? GetLoot() => new Random().Next(0, 2) == 0 ? new MysteryMeat() : null;
}

public class FleshBlob : BaseEnemy
{
    public override string Name => "Masitá koule neznámého původu";
    public override int MaxHealth => 70;
    public override int Attack => 15;
    public override int Defense => 1;
    public override int RewardCredits => 40;
    public override int SanityDamage => 15;

    public override string GetEncounterText() => "Obrovská hrouda pulsujícího masa se valí tvým směrem. Má... má to pusy. Hodně pus.";
    public override string GetAttackText() => "Blob tě polyká! Zasekl ses v ústech číslo 47!";
    public override string GetDeathText() => "Blob praská jako přezrálý meloun. Vnitřnosti jsou... no, je jich hodně.";

    public override IItem? GetLoot() => new HealthPotion();
}

public class CronenbergAlpha : BaseEnemy
{
    public override string Name => "CRONENBERG ALFA (původně Rick)";
    public override int MaxHealth => 140;
    public override int Attack => 28;
    public override int Defense => 5;
    public override int RewardCredits => 180;
    public override int SanityDamage => 25;

    public override string GetEncounterText() => "Tohle býval Rick. Teď je to obrovská inteligentní mutace, která stále... pije? 'BURP... POŘÁD JSEM... GÉNIUS, MORTY...'";
    public override string GetAttackText() => "Cronenberg-Rick po tobě stříká kyselinu z chapadel! A pořád při tom nadává!";
    public override string GetDeathText() => "'TO... BYLO... WUBBA LUBBA... DUB...' *splash*";

    public override IItem? GetLoot() => new EuphoriaPotion();
}

public class CronenbergWorld : ILocation
{
    private static readonly Random _rng = new();

    public string Name => "Svět Cronenbergů";
    public string Description => "Původně to byla Země, ale pak se někdo (Rick) vykašlal na bezpečnostní protokoly. Teď je to tu jedna velká masitá párty bez hudby.";
    public ConsoleColor ThemeColor => ConsoleColor.DarkRed;
    public bool HasBoss => true;

    public string[] AsciiArt => new[]
    {
        @"   (\(\ /)/)",
        @"   (◉益◉ )",
        @"  c('')('')",
        @"  /  MEAT  \",
        @" (  WORLD   )",
        @"  \________/"
    };

    public IEnemy SpawnEnemy()
    {
        return _rng.Next(0, 2) == 0 ? new CronenbergMonster() : new FleshBlob();
    }

    public IEnemy SpawnBoss() => new CronenbergAlpha();

    public string GetEventText()
    {
        string[] events = {
            "Šlapeš po něčem měkkém. Radši se nedívej dolů.",
            "Nacházíš starý fotku normální rodiny. Za rámečkem je maso.",
            "Něco ti mává. Není to ruka. Možná to byla ruka. Kdysi.",
            "Zaslechneš hudbu. Je to ringtone. Telefon je... součástí stvoření."
        };
        return events[_rng.Next(events.Length)];
    }

    public IItem? GetLocationItem()
    {
        if (_rng.Next(0, 3) == 0) return new MysteryMeat();
        return null;
    }
}
