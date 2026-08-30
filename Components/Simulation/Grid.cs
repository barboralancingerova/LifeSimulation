using System.Data;

public class Grid 
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Cell[,] Cells { get; private set; }
    public double SunlightIntensity { get; private set; } = 1.0; // Default sunlight intensity
    public static Random Rng = new Random();

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells[x, y] = new Cell(x, y);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException("Cell coordinates are out of bounds.");

        return Cells[x, y];
    }

    public bool PlaceOffspring(Organism offspring, int x, int y)
    {
        for (int dx = -Config.OffspringDistance; dx <= Config.OffspringDistance; dx++)
        {
            for (int dy = -Config.OffspringDistance; dy <= Config.OffspringDistance; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (nx < 0 || nx >= Width || ny < 0 || ny >= Height) continue;
                if (Cells[nx, ny].Occupant ==null)
                {
                    Cells[nx, ny].Occupant = offspring;
                    return true;
                }
            }
        }
        return false;
    }

    public bool MoveOrganism(Organism organism, int fromX, int fromY, int toX, int toY)
    {
        if (toX < 0 || toX >= Width || toY < 0 || toY >= Height) return false;
        if (Cells[toX, toY].Occupant != null) return false; 
        Cells[fromX, fromY].Occupant = null;
        Cells[toX, toY].Occupant = organism;
        return true;
    }
    public void Step()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var occupant = Cells[x, y].Occupant;
                if (occupant != null && !occupant.IsAlive)
                {
                    Cells[x, y].Nutrients = new Nutrients(occupant.Energy * Config.DecompositionFraction);
                    Cells[x, y].Occupant = null;
                    continue;
                }

                else if (occupant is Animal animal)
                {
                    var offspring = animal.Act(this, x, y);
                    if (offspring != null)
                    {
                        PlaceOffspring(offspring, x, y); // parents' location
                    } 
                }
                else if (occupant is Producer producer)
                {
                    var offspring = producer.Act(this, x, y);
                    if (offspring != null)
                    {
                        PlaceOffspring(offspring, x, y);
                    }
                }

                // vyprchavani zivin z prostredi??? doplnit Age u nutrients
            }
        }
    }



    // TESTING
    public void PrintStatus(int stepNumber)
    {
        int producers = 0, herbivores = 0, predators = 0;
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                var occupant = Cells[x, y].Occupant;
                if (occupant is Producer) producers ++;
                else if (occupant is Herbivore) herbivores ++;
                else if (occupant is Predator) predators ++;
            }
        Console.WriteLine($"Step {stepNumber}: \n Producers = {producers}, \n Herbivores = {herbivores}, \n Predators = {predators}");
    }

    public void InitializePopulation(double producerChance, double herbivoreChance, double predatorChance)
{
    for (int x = 0; x < Width; x++)
    {
        for (int y = 0; y < Height; y++)
        {
            double roll = Rng.NextDouble(); // 0.0 - 1.0

            if (roll < producerChance)
            {
                Cells[x, y].Occupant = new Producer(Config.ProducerEnergyMax * 0.5, Config.ProducerEnergyMax, Config.ProducerAgeMax, Genome.CreateRandomGenome_Producer());
            }
            else if (roll < producerChance + herbivoreChance)
            {
                Cells[x, y].Occupant = new Herbivore(Config.HerbivoreEnergyMax * 0.5, Config.HerbivoreEnergyMax, Config.HerbivoreAgeMax, Genome.CreateRandomGenome_Herbivore());
            }
            else if (roll < producerChance + herbivoreChance + predatorChance)
            {
                Cells[x, y].Occupant = new Predator(Config.PredatorEnergyMax * 0.5, Config.PredatorEnergyMax, Config.PredatorAgeMax, Genome.CreateRandomGenome_Predator());
            }
            // else: stays empty
        }
    }
}

}
