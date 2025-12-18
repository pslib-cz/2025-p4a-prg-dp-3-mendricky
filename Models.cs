namespace MDA.Core;

public enum CombatAction
{
    Attack,
    Defend,
    UseItem,
    Flee,
    UseAbility
}

public enum ItemType
{
    Healing,
    Weapon,
    Special,
    Quest
}

public enum AbilityType
{
    Offensive,
    Defensive,
    Utility
}

public interface IItem
{
    string Name { get; }
    string Description { get; }
    ItemType Type { get; }
    int Value { get; }
    bool Use(Player player, IEnemy? enemy = null);
}

public interface IAbility
{
    string Name { get; }
    string Description { get; }
    AbilityType Type { get; }
    int Cooldown { get; }
    int CurrentCooldown { get; set; }
    bool CanUse { get; }
    string Execute(Player player, IEnemy? enemy);
    void ReduceCooldown();
}

public interface IEnemy
{
    string Name { get; }
    int Health { get; }
    int MaxHealth { get; }
    int Attack { get; }
    int Defense { get; }
    int RewardCredits { get; }
    int RewardXP { get; }
    int SanityDamage { get; }
    string GetEncounterText();
    string GetAttackText();
    string GetDeathText();
    void TakeDamage(int damage);
    bool IsAlive { get; }
    IItem? GetLoot();
}

public interface ILocation
{
    string Name { get; }
    string Description { get; }
    ConsoleColor ThemeColor { get; }
    string[] AsciiArt { get; }
    IEnemy SpawnEnemy();
    IEnemy SpawnBoss();
    bool HasBoss { get; }
    string GetEventText();
    IItem? GetLocationItem();
}

public class Player
{
    public string Name { get; set; } = "Cestovatel";
    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public int Sanity { get; set; } = 100;
    public int MaxSanity { get; set; } = 100;
    public int Credits { get; set; } = 50;
    public int Attack { get; set; } = 15;
    public int Defense { get; set; } = 5;
    public int TurnsPlayed { get; set; } = 0;
    public int EnemiesDefeated { get; set; } = 0;
    public int LocationsVisited { get; set; } = 0;
    public bool HasEuphoriaPotion { get; set; } = false;
    public bool IsDefending { get; set; } = false;
    
    // New RPG stats
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int ExperienceToNextLevel => Level * 50;
    public int CriticalChance { get; set; } = 10; // Percentage
    public int DodgeChance { get; set; } = 5; // Percentage
    public int LifeSteal { get; set; } = 0; // Percentage
    public int BossesDefeated { get; set; } = 0;

    public List<IItem> Inventory { get; } = new();
    public List<IAbility> Abilities { get; } = new();

    public bool IsAlive => Health > 0 && Sanity > 0;

    public void GainExperience(int xp)
    {
        Experience += xp;
        while (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Experience -= ExperienceToNextLevel;
        Level++;
        MaxHealth += 10;
        Health = MaxHealth;
        MaxSanity += 5;
        Sanity = MaxSanity;
        Attack += 3;
        Defense += 1;
        CriticalChance = Math.Min(50, CriticalChance + 2);
    }

    public void TakeDamage(int damage)
    {
        // Check for dodge
        Random rng = new();
        if (rng.Next(0, 100) < DodgeChance)
        {
            return; // Dodged!
        }
        
        int actualDamage = IsDefending ? Math.Max(1, damage - Defense * 2) : Math.Max(1, damage - Defense);
        Health = Math.Max(0, Health - actualDamage);
        IsDefending = false;
    }

    public void TakeSanityDamage(int damage)
    {
        Sanity = Math.Max(0, Sanity - damage);
    }

    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
    }

    public void RestoreSanity(int amount)
    {
        Sanity = Math.Min(MaxSanity, Sanity + amount);
    }

    public void AddItem(IItem item)
    {
        Inventory.Add(item);
    }

    public void RemoveItem(IItem item)
    {
        Inventory.Remove(item);
    }

    public void AddAbility(IAbility ability)
    {
        if (!Abilities.Any(a => a.Name == ability.Name))
        {
            Abilities.Add(ability);
        }
    }

    public void ReduceAllCooldowns()
    {
        foreach (var ability in Abilities)
        {
            ability.ReduceCooldown();
        }
    }
}

// ABILITIES
public class PowerStrike : IAbility
{
    public string Name => "⚡ Silový úder";
    public string Description => "Mohutný úder způsobující dvojnásobné poškození.";
    public AbilityType Type => AbilityType.Offensive;
    public int Cooldown => 3;
    public int CurrentCooldown { get; set; } = 0;
    public bool CanUse => CurrentCooldown == 0;

    public string Execute(Player player, IEnemy? enemy)
    {
        if (enemy == null) return "Nemáš cíl!";
        CurrentCooldown = Cooldown;
        int damage = player.Attack * 2 + 10;
        enemy.TakeDamage(damage);
        return $"SILOVÝ ÚDER! Způsobil jsi {Math.Max(1, damage - enemy.Defense)} devastujícího poškození!";
    }

    public void ReduceCooldown() => CurrentCooldown = Math.Max(0, CurrentCooldown - 1);
}

public class MeditateAbility : IAbility
{
    public string Name => "🧘 Meditace";
    public string Description => "Obnoví 30 Sanity a 15 HP.";
    public AbilityType Type => AbilityType.Utility;
    public int Cooldown => 4;
    public int CurrentCooldown { get; set; } = 0;
    public bool CanUse => CurrentCooldown == 0;

    public string Execute(Player player, IEnemy? enemy)
    {
        CurrentCooldown = Cooldown;
        player.RestoreSanity(30);
        player.Heal(15);
        return "Meditoval jsi uprostřed boje. Divný, ale funguje to. +30 Sanity, +15 HP!";
    }

    public void ReduceCooldown() => CurrentCooldown = Math.Max(0, CurrentCooldown - 1);
}

public class VampiricBite : IAbility
{
    public string Name => "🧛 Vampýří kousnutí";
    public string Description => "Útočí a léčí za 50% způsobeného poškození.";
    public AbilityType Type => AbilityType.Offensive;
    public int Cooldown => 2;
    public int CurrentCooldown { get; set; } = 0;
    public bool CanUse => CurrentCooldown == 0;

    public string Execute(Player player, IEnemy? enemy)
    {
        if (enemy == null) return "Nemáš cíl!";
        CurrentCooldown = Cooldown;
        int damage = player.Attack + 5;
        enemy.TakeDamage(damage);
        int actualDamage = Math.Max(1, damage - enemy.Defense);
        int heal = actualDamage / 2;
        player.Heal(heal);
        return $"Koušeš nepřítele! {actualDamage} poškození, +{heal} HP vysáto!";
    }

    public void ReduceCooldown() => CurrentCooldown = Math.Max(0, CurrentCooldown - 1);
}

public class BerserkRage : IAbility
{
    public string Name => "😤 Zuřivý amok";
    public string Description => "Tripple attack, ale ztratíš 20 Sanity.";
    public AbilityType Type => AbilityType.Offensive;
    public int Cooldown => 5;
    public int CurrentCooldown { get; set; } = 0;
    public bool CanUse => CurrentCooldown == 0;

    public string Execute(Player player, IEnemy? enemy)
    {
        if (enemy == null) return "Nemáš cíl!";
        CurrentCooldown = Cooldown;
        int damage = player.Attack * 3;
        enemy.TakeDamage(damage);
        player.TakeSanityDamage(20);
        return $"ZUŘIVÝ AMOK! {Math.Max(1, damage - enemy.Defense)} poškození! Ale ztratil jsi trochu sebe... -20 Sanity!";
    }

    public void ReduceCooldown() => CurrentCooldown = Math.Max(0, CurrentCooldown - 1);
}

public class Fortify : IAbility
{
    public string Name => "🏰 Opevnění";
    public string Description => "Dočasně +10 Defense na 1 tah.";
    public AbilityType Type => AbilityType.Defensive;
    public int Cooldown => 3;
    public int CurrentCooldown { get; set; } = 0;
    public bool CanUse => CurrentCooldown == 0;

    public string Execute(Player player, IEnemy? enemy)
    {
        CurrentCooldown = Cooldown;
        player.Defense += 10;
        player.IsDefending = true;
        return "Opevňuješ se! +10 Defense pro příští útok!";
    }

    public void ReduceCooldown() => CurrentCooldown = Math.Max(0, CurrentCooldown - 1);
}

public class TimeWarp : IAbility
{
    public string Name => "⏰ Časový skok";
    public string Description => "Resetuje cooldown všech ostatních schopností.";
    public AbilityType Type => AbilityType.Utility;
    public int Cooldown => 8;
    public int CurrentCooldown { get; set; } = 0;
    public bool CanUse => CurrentCooldown == 0;

    public string Execute(Player player, IEnemy? enemy)
    {
        CurrentCooldown = Cooldown;
        foreach (var ability in player.Abilities)
        {
            if (ability != this)
            {
                ability.CurrentCooldown = 0;
            }
        }
        return "Čas se zastavil! Všechny schopnosti jsou znovu připraveny!";
    }

    public void ReduceCooldown() => CurrentCooldown = Math.Max(0, CurrentCooldown - 1);
}

// Base enemy class that implements common functionality
public abstract class BaseEnemy : IEnemy
{
    public abstract string Name { get; }
    public int Health { get; protected set; }
    public abstract int MaxHealth { get; }
    public abstract int Attack { get; }
    public abstract int Defense { get; }
    public abstract int RewardCredits { get; }
    public virtual int RewardXP => MaxHealth / 2;
    public virtual int SanityDamage => 5;

    public bool IsAlive => Health > 0;

    protected BaseEnemy()
    {
        Health = MaxHealth;
    }

    public abstract string GetEncounterText();
    public abstract string GetAttackText();
    public virtual string GetDeathText() => $"{Name} padá k zemi. Skonej.";

    public void TakeDamage(int damage)
    {
        int actualDamage = Math.Max(1, damage - Defense);
        Health = Math.Max(0, Health - actualDamage);
    }

    public virtual IItem? GetLoot() => null;
}

// Common items
public class HealthPotion : IItem
{
    public string Name => "Léčivý lektvar";
    public string Description => "Obnovuje 30 HP. Chutná to jako starej koberec, ale funguje.";
    public ItemType Type => ItemType.Healing;
    public int Value => 25;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.Heal(30);
        return true;
    }
}

public class MegaHealthPotion : IItem
{
    public string Name => "MEGA Léčivý lektvar";
    public string Description => "Obnovuje 80 HP. Zelená a bublá. Jako radioaktivní sliz.";
    public ItemType Type => ItemType.Healing;
    public int Value => 60;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.Heal(80);
        return true;
    }
}

public class SanityPill : IItem
{
    public string Name => "Prášek na klid";
    public string Description => "Obnovuje 25 sanity. Legální ve většině dimenzí.";
    public ItemType Type => ItemType.Healing;
    public int Value => 30;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.RestoreSanity(25);
        return true;
    }
}

public class AttackBoost : IItem
{
    public string Name => "Mega Steroid";
    public string Description => "Permanentně zvyšuje útok o 5. Vedlejší účinky? Jaké vedlejší účinky?";
    public ItemType Type => ItemType.Weapon;
    public int Value => 40;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.Attack += 5;
        return true;
    }
}

public class DefenseBoost : IItem
{
    public string Name => "Kosmická zbroj";
    public string Description => "Permanentně zvyšuje obranu o 3. Ze syntetického mimozemského kovu.";
    public ItemType Type => ItemType.Weapon;
    public int Value => 45;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.Defense += 3;
        return true;
    }
}

public class PortalFluid : IItem
{
    public string Name => "Portálová tekutina";
    public string Description => "Umožňuje okamžitý útěk z boje. Zelená a lesklá.";
    public ItemType Type => ItemType.Special;
    public int Value => 50;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        return true; // Flee is handled in game engine
    }
}

public class EuphoriaPotion : IItem
{
    public string Name => "🌟 BÁJNÝ NÁPOJ EUFORIE 🌟";
    public string Description => "TO JE ON! Ten chlast, co jsi hledal! Konečně se můžeš cítit... dobře?";
    public ItemType Type => ItemType.Quest;
    public int Value => 9999;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.HasEuphoriaPotion = true;
        return true;
    }
}

public class MysteryMeat : IItem
{
    public string Name => "Záhadné maso";
    public string Description => "Nevíš, co to je. Nevíš, odkud to je. Jíš to.";
    public ItemType Type => ItemType.Healing;
    public int Value => 15;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        Random rng = new();
        if (rng.Next(0, 2) == 0)
        {
            player.Heal(40);
        }
        else
        {
            player.TakeDamage(10);
            player.TakeSanityDamage(5);
        }
        return true;
    }
}

public class PlumbusCleaner : IItem
{
    public string Name => "Čistič na Plumbus";
    public string Description => "Každej ví, k čemu to je.";
    public ItemType Type => ItemType.Special;
    public int Value => 5;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.RestoreSanity(10);
        return true;
    }
}

public class ExperienceOrb : IItem
{
    public string Name => "Zkušenostní orb";
    public string Description => "Koncentrovaná zkušenost. +100 XP okamžitě.";
    public ItemType Type => ItemType.Special;
    public int Value => 75;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.GainExperience(100);
        return true;
    }
}

public class DodgeCloak : IItem
{
    public string Name => "Plášť vyhýbání";
    public string Description => "Permanentně +5% šance na uhnutí útoku.";
    public ItemType Type => ItemType.Special;
    public int Value => 80;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.DodgeChance = Math.Min(50, player.DodgeChance + 5);
        return true;
    }
}

public class CriticalGloves : IItem
{
    public string Name => "Rukavice kritických zásahů";
    public string Description => "Permanentně +5% šance na kritický zásah.";
    public ItemType Type => ItemType.Weapon;
    public int Value => 70;

    public bool Use(Player player, IEnemy? enemy = null)
    {
        player.CriticalChance = Math.Min(70, player.CriticalChance + 5);
        return true;
    }
}
