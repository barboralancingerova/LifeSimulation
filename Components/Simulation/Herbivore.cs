public class Herbivore : Animal
{
    // Constructor
    public Herbivore(double energy, double energyMax, int ageMax) : base(energy, energyMax, ageMax, SimulationConfig.HerbivoreAdultAge)
    { }

    // Methods
    public override void Eat(Organism prey)
    {
        if (prey.IsAlive && prey is Producer)
        {
            this.UpdateEnergy(prey.Energy * SimulationConfig.PredationEfficiency);
            prey.Die();
        }
    }
    public override Animal? Reproduce(Organism mate)
    {
        if (Energy / EnergyMax < SimulationConfig.ReproductionEnergyThreshold 
        || mate.Energy / mate.EnergyMax < SimulationConfig.ReproductionEnergyThreshold
        || mate is not Herbivore)
        {
            return null; // Not enough energy to reproduce
        }
        this.UpdateEnergy(-EnergyMax * SimulationConfig.ReproductionEnergyCost);
        mate.UpdateEnergy(-mate.EnergyMax * SimulationConfig.ReproductionEnergyCost);
        var offspring = new Herbivore(SimulationConfig.NewbornEnergyFraction * EnergyMax, EnergyMax, AgeMax);
        return offspring;
    }
    public override void Move(int x, int y)
    {
        // Implement movement logic for the herbivore
    }
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Producer>(observations);

    protected override Organism? FindAdjacentMate(List<ObservedCell> observations)
        => FindAdjacentMateType<Herbivore>(observations);
}