public abstract class Energy
    {
        public double EnergyAmount { get; protected set; }

        public Energy(double energyAmount)
        {
            EnergyAmount = energyAmount;
        }
    }

public class Nutrients : Energy
{
    public int Age { get; private set; } = 0;

    public Nutrients(double energyAmount) : base(energyAmount) { }

    public double Drain(double requested) // organismus odcerpava energii
    {
        double drained = Math.Min(requested, EnergyAmount);
        EnergyAmount -= drained;
        return drained;
    }

    public void Absorb(double amount) // pribyva energie z mrtvolky
{
    if (amount > 0)
    {
        EnergyAmount += amount;
    }
}

    public void Decay() //rozklada se XP
    {
        Age++;
        EnergyAmount *= Config.NutrientDecayFactor;   //exponential decay
    }

    public bool IsDepleted => EnergyAmount < Config.NutrientMinEnergy;
}
public  class Sunlight : Energy
    {
        public Sunlight(double energyAmount) : base(energyAmount)
        { }
        public void UpdateEnergy(double amount)
        {
            if (amount > Config.SunlightMaxEnergy)
            {
                EnergyAmount = Config.SunlightMaxEnergy;
            }
            else if (amount < 0)
            {
                EnergyAmount = 0;
            }
            else
            {
                EnergyAmount = amount;
            }
        }
    }