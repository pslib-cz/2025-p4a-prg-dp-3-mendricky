using MDA.Core;

namespace MDA.Locations;

public class GalacticBureaucrat : IEnemy
{
    public string Name => "Gromflomit s formulářem A-38";
    public string GetEncounterText() => "Zastaví tě úředník se šesti rukama. Chce po tobě povolení k existenci v trojrozměrném prostoru. Nemáš ho. Bude to bolet... tvoje nervy.";
}

public class BureaucraticHellscape : ILocation
{
    public string Name => "Dimenze nekonečného papírování";
    public string Description => "Všude jsou šanony. Dokonce i vzduch je tu víceméně jen rozemletej papír. Pokud sem někdy půjdeš, nezapomeň si propisku. Vlastně ne, zapomeň na všechno.";

    public IEnemy SpawnEnemy() => new GalacticBureaucrat();
}
