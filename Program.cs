using MDA.Core;
using MDA.Locations;

var engine = new GameEngine();

engine.RegisterLocation(new NebulaOfSadness());
engine.RegisterLocation(new NeonSpaceStation());
engine.RegisterLocation(new BureaucraticHellscape());
engine.RegisterLocation(new CronenbergWorld());
engine.RegisterLocation(new HiveMindPlanet());

engine.Run();
