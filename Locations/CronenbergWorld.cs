using MDA.Core;

namespace MDA.Locations;

public class CronenbergMonster : IEnemy
{
    public string Name => "Cronenbergovský Morty";
    public string GetEncounterText() => "Vypadá to jako hrouda masa, očí a zubů, co se tě snaží obejmout. Je to odporný. Je to smutný. Je to prostě vesmír.";
}

public class CronenbergWorld : ILocation
{
    public string Name => "Svět Cronenbergů";
    public string Description => "Původně to byla Země, ale pak se někdo (Rick) vykašlal na bezpečnostní protokoly. Teď je to tu jedna velká masitá párty bez hudby.";

    public IEnemy SpawnEnemy() => new CronenbergMonster();
}
