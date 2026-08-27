class Predator : Animal
{
    // Constructor
    public Predator(double energy, double energyMax, int ageMax) : base(energy, energyMax, ageMax, Config.PredatorAdultAge)
    { }

    // Methods
    public override void Eat(Organism prey)
    {
        if (prey.IsAlive && prey is Herbivore)
        {
            UpdateEnergy(prey.Energy*Config.PredationEfficiency);
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
        var offspring = new Predator(Config.NewbornEnergyFraction * EnergyMax, EnergyMax, AgeMax);
        return offspring;
    }
    public override void Move(Grid grid, int x, int y)
    {
        var directions = DirectionCoords.Coords.Values.ToList();
        var (dx, dy) = directions[Grid.Rng.Next(directions.Count)];
        //testing:
        grid.MoveOrganism(this, x, y, x + dx, y + dy);

        //TODO
        // ORIGINAL:
        /*
        if (grid.MoveOrganism(this, x, y, x + dx, y + dy))
        {
            UpdateEnergy(-Config.MovementEnergyCost*EnergyMax);
        }
        */
    }
    
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Herbivore>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Predator>(observations);
}