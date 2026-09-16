// Predítoři jsou schopni pohybu po gridu za potravou nebo partnery, živí se býložravci, nemají přirozeného nepřítele
class Predator : Animal
{
    // Constructor
    public Predator(double energy, Genome genome) : base(
        energy, 
        genome.Genes[(int)GeneIndex.EnergyMax], 
        (int)genome.Genes[(int)GeneIndex.AgeMax], 
        Config.PredatorAdultAge, 
        genome){ }
    // Methods
    public override void Eat(Organism prey)
    {
        if (prey.IsAlive && prey is Herbivore)
        {
            UpdateEnergy(prey.Energy*RuntimeSettings.PredationEfficiency);
            prey.Die();
        }
    }
    public override Animal? Reproduce(Organism mate)
    {
        if (Energy / EnergyMax < Config.ReproductionEnergyThreshold 
        || mate.Energy / mate.EnergyMax < Config.ReproductionEnergyThreshold
        || mate is not Predator)
        {
            return null; 
        }
        this.UpdateEnergy(-EnergyMax * RuntimeSettings.ReproductionEnergyCost);
        mate.UpdateEnergy(-mate.EnergyMax * RuntimeSettings.ReproductionEnergyCost);

        var offspringGenome = new AnimalGenome(4, Config.NNWeightsCount);
        offspringGenome.Crossover(this, (Animal)mate, Config.MutationSigma, 
            Config.PredatorMinAgeMax, Config.PredatorMaxAgeMax, 
            Config.PredatorMinEnergyMax, Config.PredatorMaxEnergyMax, 
            Config.PredatorMinStepEnergyCost, Config.PredatorMaxStepEnergyCost, 
            Config.PredatorMinMovementEnergyCost, Config.PredatorMaxMovementEnergyCost,
            Config.WeightBound);

        var offspring = new Predator(Config.NewbornEnergyFraction * EnergyMax, offspringGenome);
        return offspring;
    }
    
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Herbivore>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Predator>(observations);

    //
    protected override Func<ObservedCell, bool> MatchesFood()
        => cell => cell.Occupant is Herbivore;
    protected override Func<ObservedCell, bool> MatchesThreat()
        => cell => false;
    protected override Func<ObservedCell, bool> MatchesMate()
        => cell => cell.Occupant is Predator mate 
        && mate.Age >= mate.AdultAge 
        && mate.Energy / mate.EnergyMax >= Config.ReproductionEnergyThreshold;
}