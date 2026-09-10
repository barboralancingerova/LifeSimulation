public class Statistics
{

    public record Snapshot(
        int StepNumber, 
        double SolarIntensity, 
        int ProducerCount,
        int HerbivoreCount, 
        int PredatorCount, 
        double TotalEnergy, 
        double ProducerEnergy, 
        double HerbivoreEnergy, 
        double PredatorEnergy
        );
    public List<Snapshot> History { get; } = new List<Snapshot>();
    public void RecordStep(Grid grid, Sunlight sunlight, int stepNumber)
    {
        double totalEnergy = 0;
        int producers = 0, herbivores = 0, predators = 0;
        double producerEnergy = 0, herbivoreEnergy = 0, predatorEnergy = 0;
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                var occupant = grid.Cells[x, y].Occupant;
                if (occupant is Producer) 
                {
                    producers++;
                    producerEnergy += occupant.Energy;
                }
                else if (occupant is Herbivore) 
                {
                    herbivores++;
                    herbivoreEnergy += occupant.Energy;
                }
                else if (occupant is Predator) 
                {
                    predators++;
                    predatorEnergy += occupant.Energy;
                }
                if (occupant != null)
                {
                    totalEnergy += occupant.Energy;
                }
                if (grid.Cells[x, y].Nutrients != null)
                {
                    totalEnergy += grid.Cells[x, y].Nutrients.EnergyAmount;
                }
            }
        }
        var snapshot = new Snapshot(stepNumber, (double)sunlight.EnergyAmount / Config.SunlightMaxEnergy, producers, herbivores, predators, totalEnergy, producerEnergy, herbivoreEnergy, predatorEnergy);
        History.Add(snapshot);
    }
}