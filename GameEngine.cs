using MDA.Core;

namespace MDA.Core;

public class GameEngine
{
    private readonly List<ILocation> _locations = new();
    private readonly Random _random = new();

    public void RegisterLocation(ILocation location)
    {
        _locations.Add(location);
    }

    public void Run()
    {
        if (!_locations.Any())
        {
            Console.WriteLine("Žádné lokace nebyly registrovány. Vesmír je prázdný...");
            return;
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
    __  __ _    _ _      _______ _____ _____ _____ __  __ ______ _   _ ______ _____  ____  _   _          _      _   _ _____ 
   |  \/  | |  | | |    |__   __|_   _|  __ \_   _|  \/  |  ____| \ | |___  /|_   _|/ __ \| \ | |   /\   | |    | \ | |_   _|
   | \  / | |  | | |       | |    | | | |  | || | | \  / | |__  |  \| |  / /   | | | |  | |  \| |  /  \  | |    |  \| | | |  
   | |\/| | |  | | |       | |    | | | |  | || | | |\/| |  __| | . ` | / /    | | | |  | | . ` | / /\ \ | |    | . ` | | |  
   | |  | | |__| | |____   | |   _| |_| |__| || |_| |  | | |____| |\  |/ /__  _| |_| |__| | |\  |/ ____ \| |____| |\  |_| |_ 
   |_|  |_|\____/|______|  |_|  |_____|_____/|_____|_|  |_|______|_| \_/_____|_____|\____/|_| \_/_/    \_\______|_| \_|_____|
                                                                                                                             
                                      ABSŤÁK (MDA) 
                                      [Verze: Existenciární krize 1.0]
        ");
        Console.ResetColor();
        Console.WriteLine("*Grrrk* Hele, jasně... Putuješ vesmírem. Zase.");
        Console.WriteLine("Hledáš ten 'tajnej nápoj', kterej ti vymyje mozek tou sladkou euforií.");
        Console.WriteLine("Spoiler: Je to jenom chemie v mozku a jsme všichni jen smítka prachu.");
        Console.WriteLine();
        Console.WriteLine("Stiskni něco a pojďme to mít z krku.");
        Console.ReadKey(true);

        while (true)
        {
            var location = _locations[_random.Next(_locations.Count)];
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"--- LOKACE: {location.Name} ---");
            Console.ResetColor();
            Console.WriteLine(location.Description);
            Console.WriteLine();
            
            Thread.Sleep(1500);
            
            var enemy = location.SpawnEnemy();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("!!! OH SH*T, TADY TO JE !!!");
            Console.ResetColor();
            Console.WriteLine(enemy.GetEncounterText());
            Console.WriteLine();
            
            Console.WriteLine("Zneškodnil jsi ho. Gratuluju. Chceš medaili?");
            Console.WriteLine("Asi ne, ty chceš ten chlast. Tak leť dál... (Stiskni něco)");
            Console.ReadKey(true);
        }
    }
}
