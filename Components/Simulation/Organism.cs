public abstract class Organism
{
    // Properties
    public double Energy { get; private set; }
    public double EnergyMax { get; private set; }
    public bool IsAlive { get; set; }
    public int Age { get; set; }
    public int AgeMax { get; set; }
    public int AdultAge { get; set; }

    // Methods
    public void UpdateEnergy(int amount)
    {
        Energy += amount;
        if (Energy > EnergyMax)
        {
            Energy = EnergyMax;
        }
        else if (Energy < 0)
        {
            Energy = 0;
            IsAlive = false;
        }
    }    
    public void UpdateAge()
    {
        Age++;
        if (Age > AgeMax)
        {
            IsAlive = false;
        }
    }
    public void Die()
    {
        IsAlive = false;
        Energy = 0;
        // add Nutrients to the environment
    }   
    

    // Constructor
    public Organism(double Energy, double EnergyMax, int AgeMax, int AdultAge)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        this.IsAlive = true;
        this.Age = 0;
        this.AgeMax = AgeMax;
        this.AdultAge = AdultAge;
    }
}

public abstract class Animal : Organism
{
    // Constructor
    public Animal(double Energy, double EnergyMax) : base(Energy, EnergyMax)
    {
    }

    // Abstract methods for animal behavior
    public abstract void Eat(Organism prey);
    public abstract void Move(int x, int y);
    public abstract void Reproduce(Organism mate);
}