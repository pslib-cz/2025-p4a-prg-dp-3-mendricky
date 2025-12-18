using MDA.Core;

namespace MDA.Locations;

public class UnityHiveMind : BaseEnemy
{
    public override string Name => "Jednota (v těle roztomilého důchodce)";
    public override int MaxHealth => 45;
    public override int Attack => 12;
    public override int Defense => 4;
    public override int RewardCredits => 30;
    public override int SanityDamage => 22;

    public override string GetEncounterText() => "Roztomilý dědeček tě osloví hlasem deseti miliard bytostí: 'Hele, nechceš se přidat? Máme sušenky a totální ztrátu svobodné vůle.'";
    public override string GetAttackText() => "Miliarda hlasů ti křičí do hlavy: 'PŘIPOJ SE! PŘIPOJ SE! PŘIPOJ SE!'";
    public override string GetDeathText() => "Dědeček padá. Na chvíli je ticho. Pak se 9,999,999,999 hlasů směje.";

    public override IItem? GetLoot() => new SanityPill();
}

public class AssimilationDrone : BaseEnemy
{
    public override string Name => "Asimilační jednotka #7749283";
    public override int MaxHealth => 60;
    public override int Attack => 16;
    public override int Defense => 6;
    public override int RewardCredits => 40;
    public override int SanityDamage => 18;

    public override string GetEncounterText() => "Postava bez obličeje k tobě natahuje ruku. 'My jsme šťastní. Ty budeš taky. POVINNĚ.'";
    public override string GetAttackText() => "Jednotka se pokouší připojit tvůj mozek k síti! Heslo je 'POSLUŠNOST123'!";
    public override string GetDeathText() => "Jednotka se odpojuje. Její poslední myšlenka byla její první vlastní za 300 let.";
}

public class UnityCore : BaseEnemy
{
    public override string Name => "KOLEKTIVNÍ VĚDOMÍ - JÁDRO";
    public override int MaxHealth => 110;
    public override int Attack => 18;
    public override int Defense => 10;
    public override int RewardCredits => 130;
    public override int SanityDamage => 40;

    public override string GetEncounterText() => "Prostor se rozplývá. Stojíš před MYSLÍ. Miliarda očí, jedna vůle. 'VÍTEJ, FRAGMENTO. BRZY BUDEŠ CELÝ.'";
    public override string GetAttackText() => "JÁDRO ti vkládá myšlenky přímo do mozku! 'TY JSI MY. MY JSME TY.'";
    public override string GetDeathText() => "JÁDRO praská jako zrcadlo. Miliarda hlasů se rozptyluje. Miliarda bytostí poprvé přemýšlí sama.";

    public override IItem? GetLoot() => new EuphoriaPotion();
}

public class HiveMindPlanet : ILocation
{
    private static readonly Random _rng = new();

    public string Name => "Planeta Jednoty";
    public string Description => "Vypadá to jako idylické předměstí, ale všichni se na tebe usmívají úplně stejně. Úplně. Stejně.";
    public ConsoleColor ThemeColor => ConsoleColor.Cyan;
    public bool HasBoss => true;

    public string[] AsciiArt => new[]
    {
        @"   ╭──────────────────╮",
        @"   │  :)  :)  :)  :)  │",
        @"   │  :)  :)  :)  :)  │",
        @"   │  :)  :)  :)  :)  │",
        @"   │  :)  :)  :)  :)  │",
        @"   ╰──────────────────╯"
    };

    public IEnemy SpawnEnemy()
    {
        return _rng.Next(0, 2) == 0 ? new UnityHiveMind() : new AssimilationDrone();
    }

    public IEnemy SpawnBoss() => new UnityCore();

    public string GetEventText()
    {
        string[] events = {
            "Celá ulice ti synchronizovaně zamává. Přesně 47 rukou. Přesně stejný úhel.",
            "Slyšíš smích. Perfektní smích. Přesně 2.3 sekundy. Všichni najednou.",
            "Děti si hrají na hřišti. Všech 200 dělá přesně to samé.",
            "Billboard hlásá: 'ŠTĚSTÍ JE POVINNÉ. INDIVIDUALITA JE CHOROBA.'"
        };
        return events[_rng.Next(events.Length)];
    }

    public IItem? GetLocationItem()
    {
        return _rng.Next(0, 4) == 0 ? new SanityPill() : null;
    }
}
