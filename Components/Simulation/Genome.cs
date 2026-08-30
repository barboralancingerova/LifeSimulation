public enum GeneIndex
    {
        AgeMax, EnergyMax, StepEnergyCost, MovementEnergyCost
    }

public class Genome
{
    public double[] Genes {get; private set;
}

    public Genome(int genomeLength)
    {
        Genes = new double[genomeLength];
    }

    private double Recombine(double maternalGene, double paternalGene, double sigma, double min, double max)
    {
        double value = (maternalGene + paternalGene) / 2 + GaussianRNG(Grid.Rng, 0, sigma);
        return Math.Clamp(value, min, max);
    }

    public void Mutate(Organism parent, double sigma, 
        double minAgeMax, double maxAgeMax,
        double minEnergyMax, double maxEnergyMax,
        double minStepCost, double maxStepCost)
    {
        Genes[(int)GeneIndex.AgeMax] = Math.Clamp(GaussianRNG(Grid.Rng, parent.Genome.Genes[(int)GeneIndex.AgeMax], sigma), minAgeMax, maxAgeMax); 
        
        Genes[(int)GeneIndex.EnergyMax] = Math.Clamp(GaussianRNG(Grid.Rng, parent.Genome.Genes[(int)GeneIndex.EnergyMax], sigma), minEnergyMax, maxEnergyMax); 

        Genes[(int)GeneIndex.StepEnergyCost] = Math.Clamp(GaussianRNG(Grid.Rng, parent.Genome.Genes[(int)GeneIndex.StepEnergyCost], sigma), minStepCost, maxStepCost); 
    }
    public void Crossover
    (
        Animal parent1, Animal parent2, double sigma, 
        double minAgeMax, double maxAgeMax,
        double minEnergyMax, double maxEnergyMax,
        double minStepCost, double maxStepCost,
        double minMoveCost, double maxMoveCost
    )
    {
        Genes[(int)GeneIndex.AgeMax] = Recombine(
            parent1.Genome.Genes[(int)GeneIndex.AgeMax], 
            parent2.Genome.Genes[(int)GeneIndex.AgeMax], 
            sigma, minAgeMax, maxAgeMax);

        Genes[(int)GeneIndex.EnergyMax] = Recombine(
            parent1.Genome.Genes[(int)GeneIndex.EnergyMax], 
            parent2.Genome.Genes[(int)GeneIndex.EnergyMax], 
            sigma, minEnergyMax, maxEnergyMax);

        Genes[(int)GeneIndex.StepEnergyCost] = Recombine(
            parent1.Genome.Genes[(int)GeneIndex.StepEnergyCost], 
            parent2.Genome.Genes[(int)GeneIndex.StepEnergyCost], 
            sigma, minStepCost, maxStepCost);

        Genes[(int)GeneIndex.MovementEnergyCost] = Recombine(
            parent1.Genome.Genes[(int)GeneIndex.MovementEnergyCost], 
            parent2.Genome.Genes[(int)GeneIndex.MovementEnergyCost], 
            sigma, minMoveCost, maxMoveCost);
    }

    public static double GaussianRNG(Random rng, double mean, double sigma)
    {
        double u1 = rng.NextDouble();
        double u2 = rng.NextDouble();
        double z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2); // Box-Muller transform
        return mean + z * sigma;
    }

    public static Genome CreateRandomGenome_Producer()
    {
        var genome = new Genome(3);
        genome.Genes[(int)GeneIndex.AgeMax] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.ProducerAgeMax, Config.MutationSigma),
            Config.ProducerMinAgeMax, Config.ProducerMaxAgeMax);
        genome.Genes[(int)GeneIndex.EnergyMax] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.ProducerEnergyMax, Config.MutationSigma),
            Config.ProducerMinEnergyMax, Config.ProducerMaxEnergyMax);
        genome.Genes[(int)GeneIndex.StepEnergyCost] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.StepEnergyCost, Config.MutationSigma),
            Config.ProducerMinStepEnergyCost, Config.ProducerMaxStepEnergyCost);
        return genome;
    }

    public static Genome CreateRandomGenome_Herbivore()
    {
        var genome = new Genome(4);
        genome.Genes[(int)GeneIndex.AgeMax] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.HerbivoreAgeMax, Config.MutationSigma),
            Config.HerbivoreMinAgeMax, Config.HerbivoreMaxAgeMax);
        genome.Genes[(int)GeneIndex.EnergyMax] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.HerbivoreEnergyMax, Config.MutationSigma),
            Config.HerbivoreMinEnergyMax, Config.HerbivoreMaxEnergyMax);
        genome.Genes[(int)GeneIndex.StepEnergyCost] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.StepEnergyCost, Config.MutationSigma),
            Config.HerbivoreMinStepEnergyCost, Config.HerbivoreMaxStepEnergyCost);
        genome.Genes[(int)GeneIndex.MovementEnergyCost] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.MovementEnergyCost, Config.MutationSigma),
            Config.HerbivoreMinMovementEnergyCost, Config.HerbivoreMaxMovementEnergyCost);
        return genome;
    }

    public static Genome CreateRandomGenome_Predator()
    {
        var genome = new Genome(4);
        genome.Genes[(int)GeneIndex.AgeMax] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.PredatorAgeMax, Config.MutationSigma),
            Config.PredatorMinAgeMax, Config.PredatorMaxAgeMax);
        genome.Genes[(int)GeneIndex.EnergyMax] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.PredatorEnergyMax, Config.MutationSigma),
            Config.PredatorMinEnergyMax, Config.PredatorMaxEnergyMax);
        genome.Genes[(int)GeneIndex.StepEnergyCost] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.StepEnergyCost, Config.MutationSigma),
            Config.PredatorMinStepEnergyCost, Config.PredatorMaxStepEnergyCost);
        genome.Genes[(int)GeneIndex.MovementEnergyCost] = Math.Clamp(
            GaussianRNG(Grid.Rng, Config.MovementEnergyCost, Config.MutationSigma),
            Config.PredatorMinMovementEnergyCost, Config.PredatorMaxMovementEnergyCost);
        return genome;
    }
}