public class Producer: Organism
{
    // Constructor
    public Producer(int Energy, int EnergyMax, int AgeMax) : base(Energy, EnergyMax, AgeMax)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        this.AgeMax = AgeMax;
        this.AdultAge = SimulationConfig.ProducerAdultAge;
    }

    // Methods
    public void Photosynthesize()
    {
        double energyGained = SimulationConfig.PhotosynthesisEfficiency * Sunlight.EnergyAmount;
        UpdateEnergy(energyGained);
    }
    public void Reproduce()
    {
        if (Energy >= EnergyMax * SimulationConfig.ReproductionEnergyThreshold)
        {
            // Implement reproduction logic for the producer
        }
    }
}
