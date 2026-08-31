public class Herbivore : Animal
{
    // Constructor
    public Herbivore(double energy, Genome genome) : base(
        energy, 
        genome.Genes[(int)GeneIndex.EnergyMax], 
        (int)genome.Genes[(int)GeneIndex.AgeMax], 
        Config.HerbivoreAdultAge, 
        genome){ }
    // Methods
    public override void Eat(Organism prey)
    {
        if (prey.IsAlive && prey is Producer)
        {
            this.UpdateEnergy(prey.Energy * Config.PredationEfficiency);
            prey.Die();
        }
    }
    public override Animal? Reproduce(Organism mate)
    {
        if (Energy / EnergyMax < Config.ReproductionEnergyThreshold 
        || mate.Energy / mate.EnergyMax < Config.ReproductionEnergyThreshold
        || mate is not Herbivore)
        {
            return null; // Not enough energy to reproduce
        }
        this.UpdateEnergy(-EnergyMax * Config.ReproductionEnergyCost);
        mate.UpdateEnergy(-mate.EnergyMax * Config.ReproductionEnergyCost);

        var offspringGenome = new AnimalGenome(4, Config.NNWeightsCount);
        offspringGenome.Crossover(this, (Animal)mate, Config.MutationSigma, 
            Config.HerbivoreMinAgeMax, Config.HerbivoreMaxAgeMax, 
            Config.HerbivoreMinEnergyMax, Config.HerbivoreMaxEnergyMax, 
            Config.HerbivoreMinStepEnergyCost, Config.HerbivoreMaxStepEnergyCost, 
            Config.HerbivoreMinMovementEnergyCost, Config.HerbivoreMaxMovementEnergyCost,
            Config.WeightBound);

        var offspring = new Herbivore(Config.NewbornEnergyFraction * EnergyMax, offspringGenome);
        return offspring;
    }
    
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Producer>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Herbivore>(observations);

    // 
    protected override Func<ObservedCell, bool> MatchesFood()
        => cell => cell.Occupant is Producer;
    protected override Func<ObservedCell, bool> MatchesThreat()
        => cell => cell.Occupant is Predator;
    protected override Func<ObservedCell, bool> MatchesMate()
        => cell => cell.Occupant is Herbivore mate 
        && mate.Age >= mate.AdultAge 
        && mate.Energy / mate.EnergyMax >= Config.ReproductionEnergyThreshold;
}