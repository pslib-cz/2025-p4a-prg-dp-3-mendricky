using MDA.Core;

namespace MDA.Locations;

public class CyborgDealer : IEnemy
{
    public string Name => "Předraženej automat na štěstí";
    public string GetEncounterText() => "Kyber-dealer se ti snaží vnutit 'pohodu v tabletce'. Vypadá to jako paralen s příchutí mentolu. Trapný.";
}

public class NeonSpaceStation : ILocation
{
    public string Name => "Stanice 'U Poslední Naděje' (pobočka B)";
    public string Description => "Místo plné neonů, který ti vypálí sítnici, a bytostí, co mají víc končetin než IQ. Ideální místo pro to se ztratit... nebo se poblejt.";

    public IEnemy SpawnEnemy() => new CyborgDealer();
}
