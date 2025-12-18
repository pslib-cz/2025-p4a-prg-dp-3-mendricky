using MDA.Core;

namespace MDA.Locations;

public class UnityHiveMind : IEnemy
{
    public string Name => "Jednota (v těle roztomilého důchodce)";
    public string Description => "Všichni jsou součástí jedné velké kolektivní mysli. Mají skvělé pojištění, nulovou kriminalitu a hrozně se nudí.";
    public string GetEncounterText() => "Roztomilý dědeček tě osloví hlasem deseti miliard bytostí: 'Hele, nechceš se přidat? Máme sušenky a totální ztrátu svobodné vůle.'";
}

public class HiveMindPlanet : ILocation
{
    public string Name => "Planeta Jednoty";
    public string Description => "Vypadá to jako idylické předměstí, ale všichni se na tebe usmívají úplně stejně. Úplně. Stejně.";

    public IEnemy SpawnEnemy() => new UnityHiveMind();
}
