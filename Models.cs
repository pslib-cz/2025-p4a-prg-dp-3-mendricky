namespace MDA.Core;

public interface IEnemy
{
    string Name { get; }
    string GetEncounterText();
}

public interface ILocation
{
    string Name { get; }
    string Description { get; }
    IEnemy SpawnEnemy();
}
