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
            EnergyAmount = energyAmount;
        }
    }
public class Sunlight : Energy
    {
        public Sunlight(double energyAmount) : base(energyAmount)
        {
            EnergyAmount = energyAmount;
        }
        public void UpdateEnergy(double amount)
        {
            if (EnergyAmount + amount > SimulationConfig.SunlightMaxEnergy)
            {
                EnergyAmount = SimulationConfig.SunlightMaxEnergy;
            }
            else if (EnergyAmount + amount < 0)
            {
                EnergyAmount = 0;
            }
            else
            {
                EnergyAmount += amount;
            }
        }
    }