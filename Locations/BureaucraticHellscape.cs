using MDA.Core;

namespace MDA.Locations;

public class GalacticBureaucrat : BaseEnemy
{
    public override string Name => "Gromflomit s formulářem A-38";
    public override int MaxHealth => 45;
    public override int Attack => 10;
    public override int Defense => 6;
    public override int RewardCredits => 25;
    public override int SanityDamage => 25;

    public override string GetEncounterText() => "Zastaví tě úředník se šesti rukama. Chce po tobě povolení k existenci v trojrozměrném prostoru. Nemáš ho. Bude to bolet... tvoje nervy.";
    public override string GetAttackText() => "Úředník na tebe hází formuláře! Papír ti řeže kůži i duši!";
    public override string GetDeathText() => "Úředník padá na zem. Jeho poslední slova? 'Příště... vyplňte... formulář... správně...'";

    public override IItem? GetLoot() => new Random().Next(0, 4) == 0 ? new PortalFluid() : null;
}

public class AuditDrone : BaseEnemy
{
    public override string Name => "Kontrolní dron FU-2000";
    public override int MaxHealth => 35;
    public override int Attack => 16;
    public override int Defense => 8;
    public override int RewardCredits => 30;
    public override int SanityDamage => 18;

    public override string GetEncounterText() => "Dron tě skenuje. 'NALEZENO: 47 PORUŠENÍ ZÁKONA. ZAHAJUJI KOREKČNÍ PROTOKOL.'";
    public override string GetAttackText() => "Dron ti vypaluje paragraf 47b přímo do čela!";
    public override string GetDeathText() => "Dron exploduje. Z jeho útrob vypadávají důkazy o tvojí neexistenci.";
}

public class OmniDirector : BaseEnemy
{
    public override string Name => "NEJVYŠŠÍ ŘEDITEL VŠEHO";
    public override int MaxHealth => 130;
    public override int Attack => 20;
    public override int Defense => 12;
    public override int RewardCredits => 150;
    public override int SanityDamage => 35;

    public override string GetEncounterText() => "Obrovská postava v šedém obleku vstupuje. 'Tvůj čas na pohovor... vypršel před 3 miliardami let.'";
    public override string GetAttackText() => "Ředitel ti čte z nekonečného seznamu tvých administrativních selhání!";
    public override string GetDeathText() => "Ředitel se rozpadá na tisíce papírků. Někde se otevírá okno svobody.";

    public override IItem? GetLoot() => new Random().Next(0, 3) == 0 ? new EuphoriaPotion() : new PortalFluid();
}

public class BureaucraticHellscape : ILocation
{
    private static readonly Random _rng = new();

    public string Name => "Dimenze nekonečného papírování";
    public string Description => "Všude jsou šanony. Dokonce i vzduch je tu víceméně jen rozemletej papír. Pokud sem někdy půjdeš, nezapomeň si propisku. Vlastně ne, zapomeň na všechno.";
    public ConsoleColor ThemeColor => ConsoleColor.Gray;
    public bool HasBoss => true;

    public string[] AsciiArt => new[]
    {
        @"   ┌────────────────────────┐",
        @"   │ FORMULÁŘ A-38-B/2      │",
        @"   │ ☐ Životnost ☐ Smrt     │",
        @"   │ ☐ Jiné: ______________ │",
        @"   │ Podpis: X_____________ │",
        @"   └────────────────────────┘"
    };

    public IEnemy SpawnEnemy()
    {
        return _rng.Next(0, 2) == 0 ? new GalacticBureaucrat() : new AuditDrone();
    }

    public IEnemy SpawnBoss() => new OmniDirector();

    public string GetEventText()
    {
        string[] events = {
            "Čekáš ve frontě. Číslo 3 472 891. Tvoje číslo? 3 472 892. Blíží se!",
            "Automat na kafe je rozbitý. A to on byl tvoje jediná naděje.",
            "Nacházíš propisku! Funguje! To je malý zázrak v této dimenzi.",
            "Někdo křičí 'DALŠÍ!' už 3 hodiny. Nikdo nejde. Nikdy nejde."
        };
        return events[_rng.Next(events.Length)];
    }

    public IItem? GetLocationItem()
    {
        return _rng.Next(0, 5) == 0 ? new SanityPill() : null;
    }
}
