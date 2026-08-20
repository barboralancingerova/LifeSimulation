public class Producer: Organism
{
    // Constructor
    public Producer(double energy, double energyMax, int ageMax) : base(energy, energyMax, age              Max, SimulationConfig.ProducerAdultAge)
    { }

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
