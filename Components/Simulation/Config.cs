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
    public const double MutationSigma = 0.05; // 5% standard dev.

        // Producers (default: AgeMax=40, EnergyMax=1000, StepEnergyCost=0.01)
        public const double ProducerMinAgeMax = 20;
        public const double ProducerMaxAgeMax = 60;
        public const double ProducerMinEnergyMax = 500;
        public const double ProducerMaxEnergyMax = 1500;
        public const double ProducerMinStepEnergyCost = 0.005;
        public const double ProducerMaxStepEnergyCost = 0.02;

        // Herbivores (default:: AgeMax=80, EnergyMax=2000, StepEnergyCost=0.01, MovementEnergyCost=0.02)
        public const double HerbivoreMinAgeMax = 40;
        public const double HerbivoreMaxAgeMax = 120;
        public const double HerbivoreMinEnergyMax = 1000;
        public const double HerbivoreMaxEnergyMax = 3000;
        public const double HerbivoreMinStepEnergyCost = 0.005;
        public const double HerbivoreMaxStepEnergyCost = 0.02;
        public const double HerbivoreMinMovementEnergyCost = 0.01;
        public const double HerbivoreMaxMovementEnergyCost = 0.04;

        // Predators (default: AgeMax=150, EnergyMax=3500, StepEnergyCost=0.01, MovementEnergyCost=0.02)
        public const double PredatorMinAgeMax = 75;
        public const double PredatorMaxAgeMax = 225;
        public const double PredatorMinEnergyMax = 1750;
        public const double PredatorMaxEnergyMax = 5250;
        public const double PredatorMinStepEnergyCost = 0.005;
        public const double PredatorMaxStepEnergyCost = 0.02;
        public const double PredatorMinMovementEnergyCost = 0.01;
        public const double PredatorMaxMovementEnergyCost = 0.04;
    
    // NN params
    public const double WeightBound = 2;
    public const int NeuralNetworkInputCount = 33;  // nebo jiné číslo, podle skutečného součtu
    public const int NeuralNetworkHiddenCount = 16;
    public const int NeuralNetworkOutputCount = 8;
    public const int WeightsCount = (NeuralNetworkInputCount * NeuralNetworkHiddenCount) 
                                + (NeuralNetworkHiddenCount * NeuralNetworkOutputCount);

}