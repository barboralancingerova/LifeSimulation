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
    public override void Move(int x, int y)
    {
        // Implement movement logic for the predator
    }
    public override void Reproduce(Organism mate)
    {
        // Implement reproduction logic for the predator
    }
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Herbivore>(observations);
}