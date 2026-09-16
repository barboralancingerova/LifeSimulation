// třída Grid si pamatuje aktuální stav simulace a koordinuje veškeré její dění pomocí metody Step()
// = krok simulace
public class Grid 
{
    public int StepNumber { get; private set; } = 0;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Cell[,] Cells { get; private set; }
    public Sunlight Sunlight { get; private set; } = new Sunlight(Config.SunlightMaxEnergy); 
    public static Random Rng = new Random();
    public Statistics Statistics {get; }= new Statistics(); 
    public static Organism? Tracked;

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

    public void UpdateSunlight(double amount)
    {
        Sunlight.UpdateEnergy(amount);
    }
    public Cell GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException("Cell coordinates are out of bounds.");

        return Cells[x, y];
    }

    public bool PlaceOffspring(Organism offspring, int x, int y)
    {
        // sbiram vhodna policka
        var kandidati = new List<(int nx, int ny)>();
        for (int dx = -Config.OffspringDistance; dx <= Config.OffspringDistance; dx++)
        {
            for (int dy = -Config.OffspringDistance; dy <= Config.OffspringDistance; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (nx < 0 || nx >= Width || ny < 0 || ny >= Height) continue;

                bool vyhovuje = offspring is Predator //if - predatorovi vyhovuje then
                    ? Cells[nx, ny].Occupant == null || Cells[nx, ny].Occupant is Producer // then
                    : Cells[nx, ny].Occupant == null; //else - ostatnim vyhovuje jen prazdne pole

                if (vyhovuje)
                {
                    kandidati.Add((nx, ny));
                }
            }
        }

        if (kandidati.Count == 0) return false;

        // nahodny vyber umisteni potomka
        var (px, py) = kandidati[Rng.Next(kandidati.Count)];

        var old = Cells[px, py].Occupant;
        if (old is Producer)
        {
            double e = old.Energy * Config.DecompositionFraction;
            if (Cells[px, py].Nutrients != null) Cells[px, py].Nutrients.Absorb(e);
            else Cells[px, py].Nutrients = new Nutrients(e);
        }

        Cells[px, py].Occupant = offspring;
        return true;
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

                var nutrients = Cells[x, y].Nutrients;
                if (nutrients != null)
                {
                    nutrients.Decay();
                    if (nutrients.IsDepleted)
                    {
                        Cells[x, y].Nutrients = null;
                    }
                }
            }
        }
        StepNumber++;
        Statistics.RecordStep(this, Sunlight, StepNumber);
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
        if (producerChance + herbivoreChance + predatorChance > 1)
        {
            throw new ArgumentException("Součet pravděpodobností přesahuje 1.");
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                double roll = Rng.NextDouble(); // 0.0 - 1.0

                if (roll < producerChance)
                {
                    Cells[x, y].Occupant = new Producer(Config.ProducerEnergyMax * 0.5, Genome.CreateRandomGenome_Producer());
                }
                else if (roll < producerChance + herbivoreChance)
                {
                    Cells[x, y].Occupant = new Herbivore(Config.HerbivoreEnergyMax * 0.5, Genome.CreateRandomGenome_Herbivore());
                }
                else if (roll < producerChance + herbivoreChance + predatorChance)
                {
                    Cells[x, y].Occupant = new Predator(Config.PredatorEnergyMax * 0.5, Genome.CreateRandomGenome_Predator());
                }
                // else: stays empty
            }
        }
    }

}
