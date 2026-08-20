public class Grid 
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Cell[,] Cells { get; private set; }
    public double SunlightIntensity { get; private set; } = 1.0; // Default sunlight intensity

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

    public void Step()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var occupant = Cells[x, y].Occupant;
                if (occupant is Animal animal)
                {
                    animal.Act(this, x, y); // "this" for the Grid
                }
                else if (occupant is Producer producer)
                {
                    producer.Act(this, x, y);
                }
            }
        }
    }


}
