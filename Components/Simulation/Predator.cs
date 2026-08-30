class Predator : Animal
{
    // Constructor
    public Predator(double energy, double energyMax, int ageMax, Genome genome) : base(energy, energyMax, ageMax, Config.PredatorAdultAge, genome)
    { }

    // Methods
    public override void Eat(Organism prey)
    {
        if (prey.IsAlive && prey is Herbivore)
        {
            Console.WriteLine($"Predator ulovil bylozravce! Energie: {Energy}");
            UpdateEnergy(prey.Energy*Config.PredationEfficiency);
            Console.WriteLine(Energy);
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
        this.UpdateEnergy(-EnergyMax * Config.ReproductionEnergyCost);
        mate.UpdateEnergy(-mate.EnergyMax * Config.ReproductionEnergyCost);

        var offspringGenome = new Genome(4);
        offspringGenome.Crossover(this, (Animal)mate, Config.MutationSigma, 
            Config.PredatorMinAgeMax, Config.PredatorMaxAgeMax, 
            Config.PredatorMinEnergyMax, Config.PredatorMaxEnergyMax, 
            Config.PredatorMinStepEnergyCost, Config.PredatorMaxStepEnergyCost, 
            Config.PredatorMinMovementEnergyCost, Config.PredatorMaxMovementEnergyCost);

        var offspring = new Predator(Config.NewbornEnergyFraction * EnergyMax, EnergyMax, AgeMax, offspringGenome);
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
        => FindAdjacentFoodType<Herbivore>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Predator>(observations);
}