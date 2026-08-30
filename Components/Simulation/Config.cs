public static class Config
{
    // Grid params
    public const int GridWidth = 50;
    public const int GridHeight = 50;
    
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
    public const double NewbornEnergyFraction = 0.3; // Fraction of maximum energy that a newborn organism starts with
    public const double DecompositionFraction = 0.3; // Fraction of energy turning into nutrients afer death
    public const double NutrientAbsorbtionRate = 0.2; // Amount of the Producer's EnergyMax he can absorb in one step

    // Adult ages
    public const int ProducerAdultAge = 8; // Age at which producers reach adulthood
    public const int HerbivoreAdultAge = 15; // Age at which herbivores reach adulthood
    public const int PredatorAdultAge = 25; // Age at which carnivores reach adulthood

    // Scanning parameters
    public const int ScanningRadius = 4; // Radius for scanning surroundings for food and mates

    // Reproduction
    public const int OffspringDistance = 2; // Radius for placing the offspring

    // Inicialization constants
        // Maximum ages
            public const int ProducerAgeMax = 40;
            public const int HerbivoreAgeMax = 80;
            public const int PredatorAgeMax = 150;
        // Maximum of energy
            public const double ProducerEnergyMax = 1000;
            public const double HerbivoreEnergyMax = 2000;
            public const double PredatorEnergyMax = 3500;
        // Biodiversity ratio
            public const double ProducerChance = 0.3;
            public const double HerbivoreChance = 0.03;
            public const double PredatorChance = 0.003;

    // Genetic params
        // Sigma
            public const double MutationSigma = -1;
        // Producers
            public const int ProducerGenomeLength = 3;
            public const double ProducerMinAgeMax = -1;
            public const double ProducerMaxAgeMax = -1;
            public const double ProducerMinEnergyMax = -1;
            public const double ProducerMaxEnergyMax = -1;
            public const double ProducerMinStepEnergyCost = -1;
            public const double ProducerMaxStepEnergyCost = -1;
            
        // Herbivores
            public const int HerbivoreGenomeLength = 4;

            public const double HerbivoreMinAgeMax = -1;
            public const double HerbivoreMaxAgeMax = -1;
            public const double HerbivoreMinEnergyMax = -1;
            public const double HerbivoreMaxEnergyMax = -1;
            public const double HerbivoreMinStepEnergyCost = -1;
            public const double HerbivoreMaxStepEnergyCost = -1;
            public const double HerbivoreMinMovementEnergyCost = -1;
            public const double HerbivoreMaxMovementEnergyCost = -1;
        // Predators
            public const int PredatorGenomeLength = 4;

            public const double PredatorMinAgeMax = -1;
            public const double PredatorMaxAgeMax = -1;
            public const double PredatorMinEnergyMax = -1;
            public const double PredatorMaxEnergyMax = -1;
            public const double PredatorMinStepEnergyCost = -1;
            public const double PredatorMaxStepEnergyCost = -1;
            public const double PredatorMinMovementEnergyCost = -1;
            public const double PredatorMaxMovementEnergyCost = -1;
}