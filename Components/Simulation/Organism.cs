using System.Linq;
public abstract class Organism
{
    // Properties
    public double Energy { get; private set; }
    public double EnergyMax { get; }
    public bool IsAlive { get; private set; }
    public int Age { get; private set; }
    public int AgeMax { get; set; }
    public int AdultAge { get; }
    public Genome Genome {get; }

    // Constructor
    public Organism(double energy, double energyMax, int ageMax, int adultAge, Genome genome)
    {
        Energy = energy;
        EnergyMax = energyMax;
        AgeMax = ageMax;
        AdultAge = adultAge;
        IsAlive = true;
        Age = 0;
        Genome = genome;  
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
    }   
    public void Metabolize()
    {
        UpdateEnergy(-EnergyMax * Config.StepEnergyCost);
        UpdateAge();
    }
    

}

public abstract class Animal : Organism
{
    // Constructor
    public Animal(double energy, double energyMax, int ageMax, int adultAge, Genome genome) : base(energy, energyMax, ageMax, adultAge, genome)
    { }

    // Abstract methods for animal behavior
    public abstract void Eat(Organism prey);
    public abstract void Move(Grid grid, int x, int y);
    public abstract Animal? Reproduce(Organism mate);
    protected abstract Organism? FindAdjacentFood(List<ObservedCell> observations);
    protected abstract Organism? FindAdjacentMate(List<ObservedCell> observations);
    protected abstract Func<ObservedCell, bool> MatchesFood();
    protected abstract Func<ObservedCell, bool> MatchesThreat();
    protected abstract Func<ObservedCell, bool> MatchesMate();

    // Simulation step
    public Organism? Act(Grid grid, int x, int y)
    {
        // Basal metabolism
        Metabolize();

        var observations = ScanSurroundings(grid, x, y, Config.ScanningRadius);

        // Reflexes: Food
        var adjacentFood = FindAdjacentFood(observations);
        bool ate = false;
        if (adjacentFood != null)
        {
            Eat(adjacentFood);
            ate = true;
        }
        
        // Reflexes: Mating
        Organism? offspring = null;
        if (Age >= AdultAge && Energy / EnergyMax >= Config.ReproductionEnergyThreshold)
        {
            var mate = FindAdjacentMate(observations);
            if (mate != null) offspring = Reproduce(mate);
        }

        // Strategy: Movement
        if (!ate) Move(grid, x, y);

        return offspring; // Return offspring or null
    }

    // Searching for mates 
    public Dictionary<Direction, DirectionalInfo> FindMates<T>(List<ObservedCell> observations) where T : Animal
    {
        return ScanByDirection(observations,
            cell => cell.Occupant is T mate 
            && mate.Age >= mate.AdultAge 
            && mate.Energy/mate.EnergyMax >= Config.ReproductionEnergyThreshold,
            cell => cell.Occupant!.Energy);
    }

    public T? FindAdjacentMateType<T>(List<ObservedCell> observations) where T: Animal
    {
        T? adjacentMate = null;
        foreach (ObservedCell cell in observations)
        {
            if (cell.Occupant is T mate 
            && Math.Abs(cell.Dx) <= 1 && Math.Abs(cell.Dy) <= 1
            && mate.Age >= mate.AdultAge 
            && mate.Energy/mate.EnergyMax >= Config.ReproductionEnergyThreshold)
            {
                if (adjacentMate == null || adjacentMate.Energy < mate.Energy)
                {
                    adjacentMate = (T)cell.Occupant;
                }
            }
        }
        return adjacentMate;
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
    public List<ObservedCell> ScanSurroundings(Grid grid, int x, int y, int radius) // returns a list of observed cells within the specified radius
    {
        var observations = new List<ObservedCell>();
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;
                if (nx < 0 || nx >= grid.Width || ny < 0 || ny >= grid.Height) continue;

                observations.Add(new ObservedCell
                {
                    Dx = dx,
                    Dy = dy,
                    Occupant = grid.Cells[nx, ny].Occupant,
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

    public struct DirectionalInfo
    {
        public double Distance;
        public double Value; // Energy amount in that direction
    }
    public Dictionary<Direction, DirectionalInfo> ScanByDirection( // scans the surroundings and returns a dictionary of the nearest food or mates in each direction 
    List<ObservedCell> observations, 
    Func<ObservedCell, bool> matchesCandidate,   // who am I searching for
    Func<ObservedCell, double> getValue)          // what is the energy amount
    {
        var result = new Dictionary<Direction, DirectionalInfo>();

        foreach (var cell in observations)
        {
            if (!matchesCandidate(cell)) continue;

            Direction dir = GetDirection(cell.Dx, cell.Dy);
            double distance = Math.Max(Math.Abs(cell.Dx), Math.Abs(cell.Dy));

            if (!result.ContainsKey(dir) || distance < result[dir].Distance)
            {
                result[dir] = new DirectionalInfo { Distance = distance, Value = getValue(cell) };
            }
        }

        return result;
    }

    // NN Inputs
    public double[] BuildFoodInputs(Dictionary<Direction, DirectionalInfo> foodByDirection, int maxRadius)
    {
        double[] inputs = new double[16]; // 8 directions * 2 (distance and value)
        Direction[] allDirections = Enum.GetValues<Direction>();
        for (int i = 0; i < allDirections.Length; i++)
        { 
            if (foodByDirection.TryGetValue(allDirections[i], out DirectionalInfo stats)) // looks through the dictionary to find the direction and its stats
            {
                inputs[i * 2] = 1.0 - (stats.Distance / maxRadius); // Normalize distance
                inputs[i * 2 + 1] = stats.Value; // Energy value
            }
            else
            {
                inputs[i * 2] = 0; // No food in this direction
                inputs[i * 2 + 1] = 0; // => No energy value
            }
        }
        return inputs;
    }

    public double[] BuildThreatInputs(Dictionary<Direction, DirectionalInfo> threatByDirection, int maxRadius)
    {
        Direction[] allDirections = Enum.GetValues<Direction>();
        double[] inputs = new double[allDirections.Length]; 
        for (int i = 0; i < allDirections.Length; i++)
        {
            if (threatByDirection.TryGetValue(allDirections[i], out var info))
            {
                inputs[i] = 1.0 - (info.Distance / maxRadius);  // normalize - 1=close, 0=far away
            }
            else
            {
                inputs[i] = 0.0;
            }
        }

        return inputs;
    }

    public double[] BuildMateInputs(Dictionary<Direction, DirectionalInfo> mateByDirection, int maxRadius)
    {
        Direction[] allDirections = Enum.GetValues<Direction>();
        double[] inputs = new double[allDirections.Length];
        for (int i = 0; i < allDirections.Length; i++)
        {
            if (mateByDirection.TryGetValue(allDirections[i], out var info))
            {
                inputs[i] = 1.0 - (info.Distance / maxRadius);
            }
            else
            {
                inputs[i] = 0.0;
            }
        }
        return inputs;

    }

    public double[] BuildNeuralInputs(Grid grid, int x, int y)
    {
        var surroundings = ScanSurroundings(grid, x, y, Config.ScanningRadius);
        
        var foodByDirection = ScanByDirection(surroundings, MatchesFood(), cell => cell.Occupant.Energy); // Value of the food
        var threatByDirection = ScanByDirection(surroundings, MatchesThreat(), cell => 1.0); // 
        var mateByDirection = ScanByDirection(surroundings, MatchesMate(), cell => 1.0);
        
        var foodInputs = BuildFoodInputs(foodByDirection, Config.ScanningRadius);
        var threatInputs = BuildThreatInputs(threatByDirection, Config.ScanningRadius);
        var mateInputs = BuildMateInputs(mateByDirection, Config.ScanningRadius);
        
        double[] finalInput = foodInputs
            .Concat(threatInputs)
            .Concat(mateInputs)
            .Append(Energy/EnergyMax)
            .ToArray();

        return finalInput;
    }

}