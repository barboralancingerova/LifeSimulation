class Predator : Animal
{
    // Constructor
    public Predator(double Energy, double EnergyMax, int AgeMax) : base(Energy, EnergyMax, AgeMax)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        this.AgeMax = AgeMax;
        this.AdultAge = SimulationConfig.CarnivoreAdultAge;
    }

    // Methods
    public override void Move(int x, int y)
    {
        // Implement movement logic for the predator
    }
    public override void Eat(Organism prey)
    {
        if (prey is Prey)
        {
            UpdateEnergy((int)prey.Energy);
            prey.Die();
        }
    }
}