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
    public override void Move(int x, int y)
    {
        // Implement movement logic for the herbivore
    }
    public override void Reproduce(Herbivore mate)
    {
        // Implement reproduction logic for the herbivore
    }
    protected override Organism? FindAdjacentFood(List<ObservedCell> observations)
        => FindAdjacentFoodType<Producer>(observations);
}