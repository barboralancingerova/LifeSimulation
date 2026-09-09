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

    public double Drain(double requested) {
        double drained = Math.Min(requested, EnergyAmount);
        EnergyAmount -= drained;
        return drained;
    }

    public void Decay()
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