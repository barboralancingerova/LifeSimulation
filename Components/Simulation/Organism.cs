public abstract class Organism
{
    // Properties
    public double Energy { get; private set; }
    public double EnergyMax { get; }
    public bool IsAlive { get; private set; }
    public int Age { get; private set; }
    public int AgeMax { get; set; }
    public int AdultAge { get; }

    // Constructor
    public Organism(double Energy, double EnergyMax, int AgeMax, int AdultAge)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        IsAlive = true;
        Age = 0;
        this.AgeMax = AgeMax;
        this.AdultAge = AdultAge;
    }
    // Methods
    public void UpdateEnergy(double amount)
    {
        Energy += amount;
        if (Energy > EnergyMax)
        {
            Energy = EnergyMax;
        }
        else if (Energy <= 0)
        { 
            Die();
        }
    }    
    public void UpdateAge()
    {
        Age++;
        if (Age > AgeMax)
        {
            Die();
        }
    }
    public void Die()
    {
        IsAlive = false;
        Energy = 0;
        // add Nutrients to the environment
    }   
    public void Metabolize()
    {
        UpdateEnergy(-EnergyMax * SimulationConfig.StepEnergyCost);
        UpdateAge();
    }
    

}

public abstract class Animal : Organism
{
    // Constructor
    public Animal(double Energy, double EnergyMax, int AgeMax) : base(Energy, EnergyMax, AgeMax, 0)
    {
        this.Energy = Energy;
        this.EnergyMax = EnergyMax;
        this.AgeMax = AgeMax;
    }

    // Abstract methods for animal behavior
    public abstract void Eat(Organism prey);
    public abstract void Move(int x, int y);
    public abstract void Reproduce(Organism mate);
    protected abstract Organism? FindAdjacentFood(List<ObservedCell> observations);

    // Simulation step
    public void Act(Grid grid, int x, int y)
    {
        // Basal metabolism
        Metabolize();

        // Reflexes: Food
        var observarions = ScanSurroundings(x, y, SimulationConfig.ScanningRadius);
        var adjacentFood = FindAdjacentFoodType<Organism>(observations);
        if (adjacentFood != null)
        {
            Eat(adjacentFood.Occupant);
        }
        else
        {
            Move(/*NN decides where to move*/);
        }
        
        // Reflexes: Mates
        // Strategy: Movement
    }

    // Scanning surroundings

    public struct ObservedCell
    {
        public int Dx; // relative x position from the animal
        public int Dy;
        public Organism? Occupant;
    }

    public T? FindAdjacentFoodType<T>(List<ObservedCell> observations) where T : Organism
    {
        T? adjacentFood = null;
            foreach (ObservedCell cell in observations)
            {
                if (cell.Occupant is T && Math.Abs(cell.Dx) <= 1 && Math.Abs(cell.Dy) <= 1)
                {   
                    if (adjacentFood == null)
                    {   
                        adjacentFood = (T)cell.Occupant;
                    } 
                    else if (adjacentFood.Energy < cell.Occupant.Energy)
                    {
                        adjacentFood = (T)cell.Occupant;
                    }
                }
            }      
        return adjacentFood;
    }   
    public List<ObservedCell> ScanSurroundings(int x, int y, int radius) // returns a list of observed cells within the specified radius
    {
        var observations = new List<ObservedCell>();
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;
                if (nx < 0 || nx >= Grid.Width || ny < 0 || ny >= Grid.Height) continue;

                observations.Add(new ObservedCell
                {
                    Dx = dx,
                    Dy = dy,
                    Occupant = Grid.Cells[nx, ny].Occupant,
                });
            }
        }
        return observations;
    }

public enum Direction
    {
        N, NE, E, SE, S, SW, W, NW
    }
public static class DirectionCoords
    {
        public static readonly Dictionary<Direction, (int dx, int dy)> Coords = new()
        {
            { Direction.N, (0, 1) },
            { Direction.NE, (1, 1) },
            { Direction.E, (1, 0) },
            { Direction.SE, (1, -1) },
            { Direction.S, (0, -1) },
            { Direction.SW, (-1, -1) },
            { Direction.W, (-1, 0) },
            { Direction.NW, (-1, 1) }
        };
    }

    public Direction GetDirection(int dx, int dy)
    {
        double angle = Math.Atan2(dy, dx); // -pi to +pi
        double degree = angle * (180.0 / Math.PI); // -180 to +180
        if (degree < 0) degree += 360; // 0 to 360
        int index = (int)Math.Round(degree / 45.0) % 8; // 0 to 7
        Direction[] directions = { Direction.E, Direction.NE, Direction.N, Direction.NW, Direction.W, Direction.SW, Direction.S, Direction.SE };
        return directions[index];
    }

    public struct DirectionStats
    {
        public double Distance;
        public double Value; // Energy amount in that direction
    }
    public Dictionary<Direction, DirectionStats> ScanByDirection( // scans the surroundings and returns a dictionary of the nearest food or mates in each direction 
    List<ObservedCell> observations, 
    Func<ObservedCell, bool> matchesCandidate,   // who am I searching for
    Func<ObservedCell, double> getValue)          // what is the energy amount
    {
        var result = new Dictionary<Direction, DirectionStats>();

        foreach (var cell in observations)
        {
            if (!matchesCandidate(cell)) continue;

            Direction dir = GetDirection(cell.Dx, cell.Dy);
            double distance = Math.Max(Math.Abs(cell.Dx), Math.Abs(cell.Dy));

            if (!result.ContainsKey(dir) || distance < result[dir].Distance)
            {
                result[dir] = new DirectionStats { Distance = distance, Value = getValue(cell) };
            }
        }

        return result;
    }

    // Searching for food 
    public double[] BuildFoodInputs(Dictionary<Direction, DirectionStats> foodByDirection, int maxRadius)
    {
        double[] inputs = new double[16]; // 8 directions * 2 (distance and value)
        Direction[] allDirections = { Direction.E, Direction.NE, Direction.N, Direction.NW, Direction.W, Direction.SW, Direction.S, Direction.SE };
        for (int i = 0; i < allDirections.Length; i++)
        { 
            if (foodByDirection.TryGetValue(allDirections[i], out DirectionStats stats)) // looks through the dictionary to find the direction and its stats
            {
                inputs[i * 2] = 1.0 - (stats.Distance / maxRadius); // Normalize distance
                inputs[i * 2 + 1] = stats.Value; // Energy value
            }
            else
            {
                inputs[i * 2] = 0; // No food in this direction
                inputs[i * 2 + 1] = 0; // No energy value
            }
        }
        return inputs;
    }

    // Searching for mates 
    public Dictionary<Direction, DirectionStats> FindMates<T>(List<ObservedCell> observations) where T : Animal
    {
        return ScanByDirection(observations,
            cell => cell.Occupant is T mate 
            && mate.Age >= mate.AdultAge 
            && mate.Energy/mate.EnergyMax >= SimulationConfig.ReproductionEnergyThreshold,
            cell => cell.Occupant.Energy);
    }