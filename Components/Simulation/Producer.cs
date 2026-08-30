public class Producer: Organism
{
    // Constructor
    public Producer(double energy, Genome genome) : base(
        energy, 
        genome.Genes[(int)GeneIndex.EnergyMax], 
        (int)genome.Genes[(int)GeneIndex.AgeMax], 
        Config.ProducerAdultAge, 
        genome){ }

    // Methods
    public void Photosynthesize(double sunlight)
    {
        double energyGained = Config.PhotosynthesisEfficiency * sunlight;
        UpdateEnergy(energyGained);
    }
    public Producer? Reproduce()
    {
        Producer? offspring = null;
        if (Energy >= EnergyMax * Config.ReproductionEnergyThreshold)
        {
            UpdateEnergy(-Config.ReproductionEnergyCost*EnergyMax);

            var offspringGenome = new Genome(3);
            offspringGenome.Mutate(this, Config.MutationSigma,
                Config.ProducerMinAgeMax, Config.ProducerMaxAgeMax, 
                Config.ProducerMinEnergyMax, Config.ProducerMaxEnergyMax, 
                Config.ProducerMinStepEnergyCost, Config.ProducerMaxStepEnergyCost);

            offspring = new Producer(Config.NewbornEnergyFraction*EnergyMax, offspringGenome);
        }
        return offspring;
    }
    public void AbsorbNutrients(Cell cell)
    {
        if (cell.Nutrients != null)
        {
            double maxAbsorb = EnergyMax * Config.NutrientAbsorbtionRate;
            double absorbed = Math.Min(cell.Nutrients.EnergyAmount, maxAbsorb);
            UpdateEnergy(absorbed);

            if (cell.Nutrients.EnergyAmount <= 0)
            {
                cell.Nutrients = null;
            }
        }
    }

    // Action method 
    public Producer? Act(Grid grid, int x, int y)
    {
        // Metabolize and age
        Metabolize();

        // Photosynthesize
        Photosynthesize(grid.SunlightIntensity*Config.SunlightMaxEnergy);

        // Absorb Nutrients
        AbsorbNutrients(grid.Cells[x, y]);

        // Reproduction
        Producer? offspring = null;
        if (Energy >= EnergyMax * Config.ReproductionEnergyThreshold)
        {
            offspring = Reproduce();
        }

        return offspring; 
    }
}
