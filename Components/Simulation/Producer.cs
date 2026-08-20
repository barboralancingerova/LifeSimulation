public class Producer: Organism
{
    // Constructor
    public Producer(double energy, double energyMax, int ageMax) : base(energy, energyMax, ageMax, SimulationConfig.ProducerAdultAge)
    { }

    // Methods
    public void Photosynthesize(double sunlight)
    {
        double energyGained = SimulationConfig.PhotosynthesisEfficiency * sunlight;
        UpdateEnergy(energyGained);
    }
    public Producer? Reproduce()
    {
        Producer? offspring = null;
        if (Energy >= EnergyMax * SimulationConfig.ReproductionEnergyThreshold)
        {
            UpdateEnergy(-SimulationConfig.ReproductionEnergyCost*EnergyMax);
            offspring = new Producer(SimulationConfig.NewbornEnergyFraction*EnergyMax, EnergyMax, AgeMax);
        }
        return offspring;
    }

    // Action method 
    public Producer? Act(Grid grid, int x, int y)
    {
        // Metabolize and age
        Metabolize();

        // Photosynthesize
        Photosynthesize(grid.SunlightIntensity);

        // Reproduction
        Producer? offspring = null;
        if (Energy >= EnergyMax * SimulationConfig.ReproductionEnergyThreshold)
        {
            offspring = Reproduce();
        }

        return offspring; 
    }
}
