public abstract class Energy
    {
        public double EnergyAmount { get; private set; }

        public Energy(double energyAmount)
        {
            EnergyAmount = energyAmount;
        }
    }

public class Nutrients : Energy
    {
        public Nutrients(double energyAmount) : base(energyAmount)
        {
        }
    }
public class Sunlight : Energy
    {
        public Sunlight(double energyAmount) : base(energyAmount)
        {
        }
    }