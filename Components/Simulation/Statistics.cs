public class Statistics
{
    public List<int> ProducerCounts { get; } = new();
    public List<int> HerbivoreCounts { get; } = new();
    public List<int> PredatorCounts { get; } = new();

    public void RecordStep(Grid grid)
    {
        int producers = 0, herbivores = 0, predators = 0;
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                var occupant = grid.Cells[x, y].Occupant;
                if (occupant is Producer) producers++;
                else if (occupant is Herbivore) herbivores++;
                else if (occupant is Predator) predators++;
            }
        }
        ProducerCounts.Add(producers);
        HerbivoreCounts.Add(herbivores);
        PredatorCounts.Add(predators);
    }
}