using MDA.Core;

namespace MDA.Core;

public class GameEngine
{
    private readonly List<ILocation> _locations = new();
    private readonly Random _random = new();
    private Player _player = new();
    private int _bossAppearanceCounter = 0;
    private const int BOSS_APPEARANCE_INTERVAL = 5;

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

        ShowIntro();
        CharacterCreation();

        while (_player.IsAlive && !_player.HasEuphoriaPotion)
        {
            _player.TurnsPlayed++;
            var location = _locations[_random.Next(_locations.Count)];
            
            EnterLocation(location);
            
            if (!_player.IsAlive) break;
            
            // Random event
            if (_random.Next(0, 3) == 0)
            {
                ShowEvent(location);
            }

            // Find item chance
            var foundItem = location.GetLocationItem();
            if (foundItem != null)
            {
                FoundItem(foundItem);
            }

            // Enemy encounter
            _bossAppearanceCounter++;
            IEnemy enemy;
            
            if (_bossAppearanceCounter >= BOSS_APPEARANCE_INTERVAL && location.HasBoss)
            {
                _bossAppearanceCounter = 0;
                enemy = location.SpawnBoss();
                ShowBossIntro(enemy);
            }
            else
            {
                enemy = location.SpawnEnemy();
                ShowEnemyEncounter(enemy);
            }

            Combat(enemy);

            if (!_player.IsAlive) break;

            ShowPostCombat();
        }

        ShowEnding();
    }

    private void ShowIntro()
    {
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
                                      [Verze: Existenciální krize 2.0]
                                      
        ═══════════════════════════════════════════════════════════════
        ");
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ╔══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("  ║                         🌌 VÍTEJ, CESTOVATELI! 🌌                        ║");
        Console.WriteLine("  ╠══════════════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("  ║  Putuješ multivesmírem. Hledáš BÁJNÝ NÁPOJ EUFORIE - ten jediný lektvar, ║");
        Console.WriteLine("  ║  který dokáže zaplnit prázdnotu ve tvé duši (nebo alespoň na chvíli).    ║");
        Console.WriteLine("  ║                                                                          ║");
        Console.WriteLine("  ║  Dávej si pozor na:                                                      ║");
        Console.WriteLine("  ║    ❤️  ZDRAVÍ - Klesne na 0 = Game Over                                  ║");
        Console.WriteLine("  ║    🧠 PŘÍČETNOST - Klesne na 0 = Šílenství (také Game Over)              ║");
        Console.WriteLine("  ║    💰 KREDITY - Za ně si koupíš věci na přežití                          ║");
        Console.WriteLine("  ║                                                                          ║");
        Console.WriteLine("  ║  Poraz BOSSE, získej NÁPOJ EUFORIE, a... nevím... buď šťastný?           ║");
        Console.WriteLine("  ╚══════════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  [Stiskni ENTER pro pokračování...]");
        Console.ReadKey(true);
    }

    private void CharacterCreation()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ╔═══════════════════════════════════════════╗");
        Console.WriteLine("  ║        🚀 VYTVOŘENÍ POSTAVY 🚀           ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.Write("  Jak se jmenuješ, cestovateli? > ");
        string? name = Console.ReadLine();
        _player.Name = string.IsNullOrWhiteSpace(name) ? "Anonymní Existence" : name;

        Console.WriteLine();
        Console.WriteLine($"  Vítej, {_player.Name}!");
        Console.WriteLine();
        Console.WriteLine("  Vyber svůj startovní bonus:");
        Console.WriteLine("  [1] 🗡️  Bojovník (+10 Attack)");
        Console.WriteLine("  [2] 🛡️  Tank (+30 HP)");
        Console.WriteLine("  [3] 🧠 Filosof (+25 Sanity)");
        Console.WriteLine("  [4] 💰 Obchodník (+50 Credits + Léčivý lektvar)");
        Console.WriteLine();
        Console.Write("  Tvoje volba > ");

        var choice = Console.ReadKey(true);
        Console.WriteLine();

        switch (choice.KeyChar)
        {
            case '1':
                _player.Attack += 10;
                _player.AddAbility(new PowerStrike());
                _player.AddAbility(new BerserkRage());
                Console.WriteLine("  Jsi BOJOVNÍK! Tvé pěsti jsou tvá zbraň. (A někdy i nohy.)");
                Console.WriteLine("  Naučil ses: ⚡ Silový úder a 😤 Zuřivý amok!");
                break;
            case '2':
                _player.MaxHealth += 30;
                _player.Health += 30;
                _player.AddAbility(new Fortify());
                _player.AddAbility(new MeditateAbility());
                Console.WriteLine("  Jsi TANK! Můžeš schytat víc ran než průměrný Cronenberg.");
                Console.WriteLine("  Naučil ses: 🏰 Opevnění a 🧘 Meditace!");
                break;
            case '3':
                _player.MaxSanity += 25;
                _player.Sanity += 25;
                _player.AddAbility(new MeditateAbility());
                _player.AddAbility(new TimeWarp());
                Console.WriteLine("  Jsi FILOSOF! Přečetl jsi Nietzscheho a přežil. To něco znamená.");
                Console.WriteLine("  Naučil ses: 🧘 Meditace a ⏰ Časový skok!");
                break;
            case '4':
            default:
                _player.Credits += 50;
                _player.AddItem(new HealthPotion());
                _player.AddItem(new PortalFluid());
                _player.AddAbility(new VampiricBite());
                Console.WriteLine("  Jsi OBCHODNÍK! Peníze nevyřeší vše, ale pomůžou.");
                Console.WriteLine("  Naučil ses: 🧛 Vampýří kousnutí!");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("  [Stiskni ENTER pro začátek cesty...]");
        Console.ReadKey(true);
    }

    private void DrawStatusBar()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  ════════════════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        // Level and XP
        Console.Write("  ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"⭐ LVL {_player.Level} ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"[{_player.Experience}/{_player.ExperienceToNextLevel} XP]  ");
        
        // Health bar
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"❤️ {_player.Health}/{_player.MaxHealth} ");
        DrawProgressBar(_player.Health, _player.MaxHealth, ConsoleColor.Red);
        
        Console.Write("  ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"🧠 {_player.Sanity}/{_player.MaxSanity} ");
        DrawProgressBar(_player.Sanity, _player.MaxSanity, ConsoleColor.Magenta);
        Console.WriteLine();
        
        Console.Write("  ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"💰{_player.Credits}");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($" 🎒{_player.Inventory.Count}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" ⚔️{_player.Attack} 🛡️{_player.Defense}");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write($" 🎯{_player.CriticalChance}%");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write($" 💨{_player.DodgeChance}%");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($" ✨{_player.Abilities.Count} abilities");
        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  ════════════════════════════════════════════════════════════════════════");
        Console.ResetColor();
    }

    private void DrawProgressBar(int current, int max, ConsoleColor color)
    {
        int barWidth = 15;
        int filled = (int)((float)current / max * barWidth);
        
        Console.Write("[");
        Console.ForegroundColor = color;
        Console.Write(new string('█', filled));
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(new string('░', barWidth - filled));
        Console.ResetColor();
        Console.Write("]");
    }

    private void EnterLocation(ILocation location)
    {
        _player.LocationsVisited++;
        
        Console.Clear();
        DrawStatusBar();
        
        Console.ForegroundColor = location.ThemeColor;
        Console.WriteLine();
        Console.WriteLine($"  ╔══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"  ║  📍 LOKACE: {location.Name.PadRight(60)} ║");
        Console.WriteLine($"  ╚══════════════════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        
        foreach (var line in location.AsciiArt)
        {
            Console.WriteLine($"  {line}");
        }
        
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  {location.Description}");
        Console.WriteLine();
        
        Thread.Sleep(1500);
    }

    private void ShowEvent(ILocation location)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ╭─────────────────────────────────────────╮");
        Console.WriteLine("  │           ⚡ NÁHODNÁ UDÁLOST ⚡          │");
        Console.WriteLine("  ╰─────────────────────────────────────────╯");
        Console.ResetColor();
        Console.WriteLine($"  {location.GetEventText()}");
        Console.WriteLine();
        Thread.Sleep(1500);
    }

    private void FoundItem(IItem item)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ╭─────────────────────────────────────────╮");
        Console.WriteLine("  │           🎁 NALEZEN PŘEDMĚT! 🎁        │");
        Console.WriteLine("  ╰─────────────────────────────────────────╯");
        Console.ResetColor();
        Console.WriteLine($"  Našel jsi: {item.Name}");
        Console.WriteLine($"  {item.Description}");
        _player.AddItem(item);
        Console.WriteLine();
        Thread.Sleep(1000);
    }

    private void ShowEnemyEncounter(IEnemy enemy)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  ╔══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("  ║                        ⚠️  NEPŘÍTEL SPATŘEN! ⚠️                          ║");
        Console.WriteLine("  ╚══════════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  >>> {enemy.Name} <<<");
        Console.ResetColor();
        Console.WriteLine($"  {enemy.GetEncounterText()}");
        Console.WriteLine();
        Thread.Sleep(1000);
    }

    private void ShowBossIntro(IEnemy boss)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(@"
  ██████╗  ██████╗ ███████╗███████╗    ███████╗██╗ ██████╗ ██╗  ██╗████████╗██╗
  ██╔══██╗██╔═══██╗██╔════╝██╔════╝    ██╔════╝██║██╔════╝ ██║  ██║╚══██╔══╝██║
  ██████╔╝██║   ██║███████╗███████╗    █████╗  ██║██║  ███╗███████║   ██║   ██║
  ██╔══██╗██║   ██║╚════██║╚════██║    ██╔══╝  ██║██║   ██║██╔══██║   ██║   ╚═╝
  ██████╔╝╚██████╔╝███████║███████║    ██║     ██║╚██████╔╝██║  ██║   ██║   ██╗
  ╚═════╝  ╚═════╝ ╚══════╝╚══════╝    ╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚═╝
        ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  >>> {boss.Name} <<<");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  {boss.GetEncounterText()}");
        Console.WriteLine();
        Console.WriteLine("  [POZOR: Toto je BOSS! Připrav se na těžký boj!]");
        Console.WriteLine();
        Thread.Sleep(2000);
    }

    private void Combat(IEnemy enemy)
    {
        while (enemy.IsAlive && _player.IsAlive)
        {
            Console.Clear();
            DrawStatusBar();
            DrawEnemyStatus(enemy);
            DrawAbilities();
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  ╭─────────────────────────────────────────╮");
            Console.WriteLine("  │          ⚔️  BOJOVÉ AKCE ⚔️              │");
            Console.WriteLine("  ├─────────────────────────────────────────┤");
            Console.WriteLine("  │  [1] 🗡️  ÚTOK                           │");
            Console.WriteLine("  │  [2] 🛡️  OBRANA (snižuje poškození)     │");
            Console.WriteLine("  │  [3] 🎒 POUŽÍT PŘEDMĚT                  │");
            Console.WriteLine("  │  [4] 🏃 UTÉCT                            │");
            Console.WriteLine("  │  [5] ✨ POUŽÍT SCHOPNOST                │");
            Console.WriteLine("  ╰─────────────────────────────────────────╯");
            Console.ResetColor();
            Console.Write("  Tvoje akce > ");

            var action = Console.ReadKey(true);
            Console.WriteLine();
            bool actionTaken = true;

            switch (action.KeyChar)
            {
                case '1':
                    PlayerAttack(enemy);
                    break;
                case '2':
                    PlayerDefend();
                    break;
                case '3':
                    if (!UseItem(enemy))
                    {
                        Console.WriteLine("  Nemáš žádné předměty!");
                        Thread.Sleep(1000);
                        actionTaken = false;
                    }
                    break;
                case '4':
                    if (TryFlee())
                    {
                        Console.WriteLine("  Utekl jsi! Jako správný zbabělec.");
                        Thread.Sleep(1500);
                        return;
                    }
                    Console.WriteLine("  Útěk selhal! Nepřítel tě chytil!");
                    break;
                case '5':
                    if (!UseAbility(enemy))
                    {
                        actionTaken = false;
                    }
                    break;
                default:
                    actionTaken = false;
                    continue;
            }

            // Reduce cooldowns after action
            if (actionTaken)
            {
                _player.ReduceAllCooldowns();
            }

            // Enemy turn
            if (enemy.IsAlive && _player.IsAlive && actionTaken)
            {
                EnemyAttack(enemy);
            }

            Thread.Sleep(1000);
        }

        if (!enemy.IsAlive && _player.IsAlive)
        {
            EnemyDefeated(enemy);
        }
    }

    private void DrawAbilities()
    {
        if (!_player.Abilities.Any()) return;
        
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  ✨ Schopnosti: ");
        foreach (var ability in _player.Abilities)
        {
            if (ability.CanUse)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{ability.Name}] ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{ability.Name} ({ability.CurrentCooldown})] ");
            }
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    private bool UseAbility(IEnemy enemy)
    {
        if (!_player.Abilities.Any())
        {
            Console.WriteLine("  Nemáš žádné schopnosti!");
            Thread.Sleep(1000);
            return false;
        }

        Console.Clear();
        Console.WriteLine("  ╔═══════════════════════════════════════════╗");
        Console.WriteLine("  ║           ✨ TVÉ SCHOPNOSTI ✨            ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════╝");
        Console.WriteLine();

        for (int i = 0; i < _player.Abilities.Count; i++)
        {
            var ability = _player.Abilities[i];
            if (ability.CanUse)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  [{i + 1}] {ability.Name} - PŘIPRAVENO");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  [{i + 1}] {ability.Name} - COOLDOWN: {ability.CurrentCooldown} tahů");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      {ability.Description}");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.WriteLine("  [0] Zpět");
        Console.Write("  Vyber schopnost > ");

        var choice = Console.ReadKey(true);
        Console.WriteLine();

        if (choice.KeyChar == '0') return false;

        if (int.TryParse(choice.KeyChar.ToString(), out int index) && index >= 1 && index <= _player.Abilities.Count)
        {
            var ability = _player.Abilities[index - 1];
            if (!ability.CanUse)
            {
                Console.WriteLine($"  Schopnost není připravena! Počkej {ability.CurrentCooldown} tahů.");
                Thread.Sleep(1000);
                return false;
            }

            string result = ability.Execute(_player, enemy);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  ✨ {result}");
            Console.ResetColor();
            return true;
        }

        return false;
    }

    private void DrawEnemyStatus(IEnemy enemy)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ╭───────────────────────────────────────────────────────────╮");
        Console.WriteLine($"  │  👾 {enemy.Name.PadRight(53)} │");
        Console.Write($"  │  HP: {enemy.Health}/{enemy.MaxHealth} ");
        DrawProgressBar(enemy.Health, enemy.MaxHealth, ConsoleColor.Red);
        Console.WriteLine($"  ATK: {enemy.Attack}  DEF: {enemy.Defense}".PadRight(30) + " │");
        Console.WriteLine($"  ╰───────────────────────────────────────────────────────────╯");
        Console.ResetColor();
    }

    private void PlayerAttack(IEnemy enemy)
    {
        int baseDamage = _player.Attack + _random.Next(-3, 6);
        bool isCrit = _random.Next(0, 100) < _player.CriticalChance;
        int damage = isCrit ? (int)(baseDamage * 1.5) : baseDamage;
        enemy.TakeDamage(damage);
        
        if (isCrit)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  💥 KRITICKÝ ZÁSAH! Způsobil jsi {Math.Max(1, damage - enemy.Defense)} poškození!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ⚔️  Útočíš na {enemy.Name}! Způsobil jsi {Math.Max(1, damage - enemy.Defense)} poškození!");
        }
        Console.ResetColor();
    }

    private void PlayerDefend()
    {
        _player.IsDefending = true;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("  🛡️  Zaujímáš obranný postoj! Další útok bude oslabený.");
        Console.ResetColor();
    }

    private bool UseItem(IEnemy? enemy)
    {
        if (!_player.Inventory.Any())
        {
            return false;
        }

        Console.Clear();
        Console.WriteLine("  ╔═══════════════════════════════════════════╗");
        Console.WriteLine("  ║             🎒 TVŮ INVENTÁŘ 🎒            ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════╝");
        Console.WriteLine();

        for (int i = 0; i < _player.Inventory.Count; i++)
        {
            var item = _player.Inventory[i];
            Console.WriteLine($"  [{i + 1}] {item.Name}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      {item.Description}");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.WriteLine("  [0] Zpět");
        Console.Write("  Vyber předmět > ");

        var choice = Console.ReadKey(true);
        Console.WriteLine();

        if (choice.KeyChar == '0') return false;

        if (int.TryParse(choice.KeyChar.ToString(), out int index) && index >= 1 && index <= _player.Inventory.Count)
        {
            var item = _player.Inventory[index - 1];
            item.Use(_player, enemy);
            _player.RemoveItem(item);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✔️  Použil jsi {item.Name}!");
            Console.ResetColor();
            return true;
        }

        return false;
    }

    private bool TryFlee()
    {
        // Check for portal fluid
        var portalFluid = _player.Inventory.FirstOrDefault(i => i is PortalFluid);
        if (portalFluid != null)
        {
            _player.RemoveItem(portalFluid);
            return true;
        }

        return _random.Next(0, 3) == 0; // 33% base flee chance
    }

    private void EnemyAttack(IEnemy enemy)
    {
        int damage = enemy.Attack + _random.Next(-2, 4);
        _player.TakeDamage(damage);
        _player.TakeSanityDamage(enemy.SanityDamage);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  💥 {enemy.GetAttackText()}");
        Console.WriteLine($"  Obdržel jsi {Math.Max(1, damage - (_player.IsDefending ? _player.Defense * 2 : _player.Defense))} poškození a -{enemy.SanityDamage} Sanity!");
        Console.ResetColor();
    }

    private void EnemyDefeated(IEnemy enemy)
    {
        _player.EnemiesDefeated++;
        _player.Credits += enemy.RewardCredits;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ╔══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("  ║                          🏆 VÍTĚZSTVÍ! 🏆                                ║");
        Console.WriteLine("  ╚══════════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  {enemy.GetDeathText()}");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  💰 Získal jsi {enemy.RewardCredits} kreditů!");
        Console.ResetColor();

        var loot = enemy.GetLoot();
        if (loot != null)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"  🎁 Nepřítel upustil: {loot.Name}!");
            Console.ResetColor();
            _player.AddItem(loot);

            if (loot is EuphoriaPotion)
            {
                _player.HasEuphoriaPotion = true;
            }
        }

        Thread.Sleep(2000);
    }

    private void ShowPostCombat()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  ╭─────────────────────────────────────────╮");
        Console.WriteLine("  │         🛒 RYCHLÝ ODPOČINEK 🛒          │");
        Console.WriteLine("  ╰─────────────────────────────────────────╯");
        Console.ResetColor();
        Console.WriteLine("  [1] 💊 Koupit Léčivý lektvar (25 kreditů, +30 HP)");
        Console.WriteLine("  [2] 💊 Koupit Prášek na klid (30 kreditů, +25 Sanity)");
        Console.WriteLine("  [3] 🎒 Použít předmět z inventáře");
        Console.WriteLine("  [ENTER] Pokračovat na další lokaci");
        Console.WriteLine();
        Console.Write("  > ");

        var choice = Console.ReadKey(true);

        switch (choice.KeyChar)
        {
            case '1':
                if (_player.Credits >= 25)
                {
                    _player.Credits -= 25;
                    _player.AddItem(new HealthPotion());
                    Console.WriteLine("\n  Koupil jsi Léčivý lektvar!");
                }
                else
                {
                    Console.WriteLine("\n  Nemáš dost kreditů!");
                }
                Thread.Sleep(1000);
                break;
            case '2':
                if (_player.Credits >= 30)
                {
                    _player.Credits -= 30;
                    _player.AddItem(new SanityPill());
                    Console.WriteLine("\n  Koupil jsi Prášek na klid!");
                }
                else
                {
                    Console.WriteLine("\n  Nemáš dost kreditů!");
                }
                Thread.Sleep(1000);
                break;
            case '3':
                UseItem(null);
                break;
        }
    }

    private void ShowEnding()
    {
        Console.Clear();

        if (_player.HasEuphoriaPotion)
        {
            // Victory!
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
  ██╗   ██╗██╗████████╗███████╗███████╗███████╗████████╗██╗   ██╗██╗
  ██║   ██║██║╚══██╔══╝██╔════╝╚══███╔╝██╔════╝╚══██╔══╝██║   ██║██║
  ██║   ██║██║   ██║   █████╗    ███╔╝ ███████╗   ██║   ██║   ██║██║
  ╚██╗ ██╔╝██║   ██║   ██╔══╝   ███╔╝  ╚════██║   ██║   ╚██╗ ██╔╝╚═╝
   ╚████╔╝ ██║   ██║   ███████╗███████╗███████║   ██║    ╚████╔╝ ██╗
    ╚═══╝  ╚═╝   ╚═╝   ╚══════╝╚══════╝╚══════╝   ╚═╝     ╚═══╝  ╚═╝
            ");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  🌟 NAŠEL JSI BÁJNÝ NÁPOJ EUFORIE! 🌟");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"  Cestovateli {_player.Name}, dokázal jsi to.");
            Console.WriteLine("  Po všech těch dimenzích, po všech těch příšerách...");
            Console.WriteLine("  Držíš v rukou tekutinu, která slibuje štěstí.");
            Console.WriteLine();
            Console.WriteLine("  Piješ...");
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  A? Cítíš se... dobře?");
            Console.WriteLine("  Možná. Na chvíli.");
            Console.WriteLine("  Nebo je to jen chemie v mozku.");
            Console.WriteLine("  Každopádně - vyhrál jsi. Gratuluju.");
            Console.ResetColor();
        }
        else if (_player.Health <= 0)
        {
            // Death by damage
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
   ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ 
  ██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗
  ██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝
  ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗
  ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║
   ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝
            ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"  {_player.Name} zemřel.");
            Console.WriteLine("  Multivesmír pokračuje dál, bez tebe.");
            Console.WriteLine("  Ten nápoj euforie? Někdo jiný ho najde. Nebo ne.");
            Console.WriteLine("  Na tobě už nezáleží.");
        }
        else
        {
            // Death by insanity
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"
  ███████╗██╗██╗     ███████╗███╗   ██╗███████╗████████╗██╗   ██╗██╗
  ██╔════╝██║██║     ██╔════╝████╗  ██║██╔════╝╚══██╔══╝██║   ██║██║
  ███████╗██║██║     █████╗  ██╔██╗ ██║███████╗   ██║   ██║   ██║██║
  ╚════██║██║██║     ██╔══╝  ██║╚██╗██║╚════██║   ██║   ╚██╗ ██╔╝╚═╝
  ███████║██║███████╗███████╗██║ ╚████║███████║   ██║    ╚████╔╝ ██╗
  ╚══════╝╚═╝╚══════╝╚══════╝╚═╝  ╚═══╝╚══════╝   ╚═╝     ╚═══╝  ╚═╝
            ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"  {_player.Name} ztratil příčetnost.");
            Console.WriteLine("  Tvá mysl se rozpadla na tisíce kousků.");
            Console.WriteLine("  Teď jsi jen dalším podivným stvořením multivesmíru.");
            Console.WriteLine("  Možná jsi šťastný. Těžko říct. Už nic nevíš.");
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  ═══════════════════════════════════════════════════════════════════════");
        Console.WriteLine($"  📊 STATISTIKY:");
        Console.WriteLine($"      Navštíveno lokací: {_player.LocationsVisited}");
        Console.WriteLine($"      Poraženo nepřátel: {_player.EnemiesDefeated}");
        Console.WriteLine($"      Tahů odehráno: {_player.TurnsPlayed}");
        Console.WriteLine($"      Kreditů získáno: {_player.Credits}");
        Console.WriteLine("  ═══════════════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  Díky za hraní MDA - MULTIDIMENZIONÁLNÍ ABSŤÁK!");
        Console.WriteLine("  [Stiskni ENTER pro ukončení...]");
        Console.ReadKey(true);
    }
}
