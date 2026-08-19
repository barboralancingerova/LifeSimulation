public static class SimulationConfig
{
    // Energy transfer efficiencies
    public static double PredationEfficiency = 0.3; // Efficiency of energy transfer from prey to predator
    public static const double PhotosynthesisEfficiency = 0.4; // Efficiency of converting sunlight into energy for producers
    
    // Sunlight and energy parameters
    public const int DaysToFullEnergy = 14; // Number of days it takes for a producer to reach full energy through photosynthesis
    public static double SunlightIntensity = 1.0; // Intensity of sunlight affecting photosynthesis
    public static const int SunlightMaxEnergy = 200;

    // Energy costs & thresholds
    public static const double StepEnergyCost = 0.01; // Basal energy cost for each step of the simulation
    public static const double MovementEnergyCost = 0.02; // Energy cost for movement
    public static const double ReproductionEnergyCost = 0.3; // Energy cost for reproduction
    public static const double ReproductionEnergyThreshold = 0.8; // Minimum energy level required for reproduction

    // Adult ages
    public static const int ProducerAdultAge = 8; // Age at which producers reach adulthood
    public static const int HerbivoreAdultAge = 15; // Age at which herbivores reach adulthood
    public static const int CarnivoreAdultAge = 25; // Age at which carnivores reach adulthood
}