namespace PatternsLab.Problems.Prototype;

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
}

public abstract class Enemy
{
    private string _modelData;

    public string Name { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public List<string> Abilities { get; set; } = new();
    public string ModelId => _modelData;

    protected Enemy()
    {
        Console.WriteLine("   ...loading 3D model (slow)...");
        Thread.Sleep(500);
        _modelData = "MODEL_" + Guid.NewGuid().ToString("N")[..6];

    }


    // tHIS IS A COPY CONSTRUCTOR MADE TO AVOID CALLING THE DEFAULT CONSTRUCTOR WHILE CLONING
    public Enemy(Enemy source)
    {
        _modelData = source._modelData;
        Name = source.Name;
        Health = source.Health;
        Weapon = new Weapon
        {

            Name = source.Weapon.Name,
            Damage = source.Weapon.Damage
        };

        Abilities = new List<string>(source.Abilities);

    }


    // clone made abstract to override it in subclasses so each sub class can make it's own copy
    public abstract Enemy Clone();




}

public class Orc : Enemy
{
    public Orc()
    {
        Name = "Orc";
        Health = 100;
        Weapon = new Weapon { Name = "Axe", Damage = 25 };
        Abilities.Add("Rage");
    }

    // SENDING THE SAME OBJECT TO THE BASE CLASS COPY CONSTRUCTOR TO COPY DATA
    // SO WHY SENDING DATA TO BASE CLASS COPY CONSTRUCTOR ???
    // BECAUSE PRIVATE FIELDS CANNOT BE USED OUTSIDE IT'S CLASS 
    private Orc(Enemy source) : base(source) { }

    // CLONE HERE USED TO RETURN A NEW OBJECT OF ORC BUT NOT CALLING THE DEFAULT CONSTRACTOR
    // WHICH CALLING THE BASE CLASS DEFUAKT CONSTRUCTOR
    public override Enemy Clone() => new Orc(this);



}

public class Elf : Enemy
{
    public Elf()
    {
        Name = "Elf";
        Health = 70;
        Weapon = new Weapon { Name = "Bow", Damage = 18 };
        Abilities.Add("Stealth");
    }


    private Elf(Enemy source) : base(source) { }

    public override Enemy Clone() => new Elf(this);
}

public static class EnemyCopyHelper
{
    public static Enemy CopyEnemy(Enemy e)
    {
        Enemy c;
        if (e is Orc) c = new Orc();
        else if (e is Elf) c = new Elf();
        else throw new NotSupportedException("Unknown enemy type");


        c.Name = e.Name;
        c.Health = e.Health;
        c.Weapon = e.Weapon;
        c.Abilities = e.Abilities;
        return c;
    }
}



