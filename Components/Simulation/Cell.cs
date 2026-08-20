public class Cell
{
    public Organism? Occupant { get; set; }
    public Nutrients? Nutrients { get; set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
        Occupant = null;
        Nutrients = null;    
    }
}