class Predator : Animal
{
    // Constructor
    public Predator(double energy, double energyMax, int ageMax) : base(energy, energyMax, ageMax, SimulationConfig.PredatorAdultAge)
    { }

    // Methods
    public override void Eat(Organism prey)
    {
        if (prey.IsAlive && prey is Herbivore)
        {
            UpdateEnergy(prey.Energy*SimulationConfig.PredationEfficiency);
            prey.Die();
        }
    }
    public override Animal? Reproduce(Organism mate)
    {
        if (Energy / EnergyMax < SimulationConfig.ReproductionEnergyThreshold 
        || mate.Energy / mate.EnergyMax < SimulationConfig.ReproductionEnergyThreshold
        || mate is not Predator)
        {
            return null; 
        }
        this.UpdateEnergy(-EnergyMax * SimulationConfig.ReproductionEnergyCost);
        mate.UpdateEnergy(-mate.EnergyMax * SimulationConfig.ReproductionEnergyCost);
        var offspring = new Predator(SimulationConfig.NewbornEnergyFraction * EnergyMax, EnergyMax, AgeMax);
        return offspring;
    }
    public override void Move(int x, int y)
    {
        // Implement movement logic for the predator
    }
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Herbivore>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Predator>(observations);
}