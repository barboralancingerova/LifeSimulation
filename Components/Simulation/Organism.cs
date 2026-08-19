public abstract class Organism
{
    // Properties
    public double Energy { get; private set; }
    public double EnergyMax { get; }
    public bool IsAlive { get; private set; }
    public int Age { get; private set; }
    public int AgeMax { get; set; }
    public int AdultAge { get; }

    // Methods
    public void UpdateEnergy(int amount)
    {
        Energy += amount;
        if (Energy > EnergyMax)
        {
            Energy = EnergyMax;
        }
        else if (Energy <= 0)
        { 
            Die();
        }
    }    
    public void UpdateAge()
    {
        Age++;
        if (Age > AgeMax)
        {
            Die();
        }
    }
    public void Die()
    {
        IsAlive = false;
        Energy = 0;
        // add Nutrients to the environment
    }   
    public void Metabolize()
    {
        UpdateEnergy(-AgeMax * SimulationConfig.StepEnergyCost);
        UpdateAge();
    }
    

    // Constructor
    public Organism(double Energy, double EnergyMax, int AgeMax, int AdultAge)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        IsAlive = true;
        Age = 0;
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