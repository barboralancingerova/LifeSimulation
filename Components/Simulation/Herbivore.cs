public class Herbivore : Animal
{
    // Constructor
    public Herbivore(double energy, double energyMax, int ageMax, Genome genome) : base(energy, energyMax, ageMax, Config.HerbivoreAdultAge, genome)
    { }

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

        var offspringGenome = new Genome(4);
        offspringGenome.Crossover(this, (Animal)mate, Config.MutationSigma, 
            Config.HerbivoreMinAgeMax, Config.HerbivoreMaxAgeMax, 
            Config.HerbivoreMinEnergyMax, Config.HerbivoreMaxEnergyMax, 
            Config.HerbivoreMinStepEnergyCost, Config.HerbivoreMaxStepEnergyCost, 
            Config.HerbivoreMinMovementEnergyCost, Config.HerbivoreMaxMovementEnergyCost);

        var offspring = new Herbivore(Config.NewbornEnergyFraction * EnergyMax, EnergyMax, AgeMax, offspringGenome);
        return offspring;
    }
    public override void Move(Grid grid, int x, int y)
    {
        var directions = DirectionCoords.Coords.Values.ToList();
        var (dx, dy) = directions[Grid.Rng.Next(directions.Count)];
        if (grid.MoveOrganism(this, x, y, x + dx, y + dy))
        {
            UpdateEnergy(-Config.MovementEnergyCost*EnergyMax);
        }
    }
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Producer>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Herbivore>(observations);
}