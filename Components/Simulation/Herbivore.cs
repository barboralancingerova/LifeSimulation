public class Herbivore : Animal
{
    // Constructor
    public Herbivore(double Energy, double EnergyMax) : base(Energy, EnergyMax)
    {
    }

    // Methods
    public override void Eat(Producer prey)
    {
        if (prey.IsAlive)
        {
            this.UpdateEnergy(prey.Energy * 0.3); // Herbivores gain 30% of prey's energy
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