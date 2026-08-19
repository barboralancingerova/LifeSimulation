public class Herbivore : Animal
{
    // Constructor
    public Herbivore(double Energy, double EnergyMax, int AgeMax) : base(Energy, EnergyMax, AgeMax)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        this.AgeMax = AgeMax;
        this.AdultAge = SimulationConfig.HerbivoreAdultAge;
    }

    // Methods
    public override void Eat(Producer prey)
    {
        if (prey.IsAlive)
        {
            this.UpdateEnergy(prey.Energy * SimulationConfig.PredationEfficiency);
            prey.Die();
        }
    }
    public override void Move(int x, int y)
    {
        // Implement movement logic for the herbivore
    }
    public override void Reproduce()
    {
        // Implement reproduction logic for the herbivore
    }
}