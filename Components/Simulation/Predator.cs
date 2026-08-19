class Predator : Animal
{
    public Predator(double Energy, double EnergyMax) : base(Energy, EnergyMax)
    {
        this.AgeMax = 20;
        this.AdultAge = 5;
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