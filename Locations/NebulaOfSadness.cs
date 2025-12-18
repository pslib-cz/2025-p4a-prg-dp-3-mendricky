using MDA.Core;

namespace MDA.Locations;

public class VoidSpecter : IEnemy
{
    public string Name => "Fyzikální anomálie s depresí";
    public string GetEncounterText() => "Z mlhoviny se vynořuje cosi, co vypadá jako tvoje nezaplacené účty. Má to chapadla a velmi nízké sebevědomí.";
}

public class NebulaOfSadness : ILocation
{
    public string Name => "Dimenze zbytečných emocí";
    public string Description => "Tuhle dimenzi vytvořil nějakej ufňukanej bůh, kterej neměl na Netflix. Všichni jsou tu smutní a smrdí to tu jako starej sklep.";

    public IEnemy SpawnEnemy() => new VoidSpecter();
}
