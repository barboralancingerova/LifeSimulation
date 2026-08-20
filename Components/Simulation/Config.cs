public static class SimulationConfig
{
    // Energy transfer efficiencies
    public const double PredationEfficiency = 0.3; // Efficiency of energy transfer from prey to predator
    public const double PhotosynthesisEfficiency = 0.4; // Efficiency of converting sunlight into energy for producers
    
    // Sunlight and energy parameters
    public const int DaysToFullEnergy = 14; // Number of days it takes for a producer to reach full energy through photosynthesis
    public const int SunlightMaxEnergy = 200;

    // Energy costs & thresholds
    public const double StepEnergyCost = 0.01; // Basal energy cost for each step of the simulation
    public  const double MovementEnergyCost = 0.02; // Energy cost for movement
    public const double ReproductionEnergyCost = 0.3; // Energy cost for reproduction
    public const double ReproductionEnergyThreshold = 0.8; // Minimum energy level required for reproduction

    // Adult ages
    public const int ProducerAdultAge = 8; // Age at which producers reach adulthood
    public const int HerbivoreAdultAge = 15; // Age at which herbivores reach adulthood
    public const int PredatorAdultAge = 25; // Age at which carnivores reach adulthood

    // Scanning parameters
    public const int ScanningRadius = 4; // Radius for scanning surroundings for food and mates
}