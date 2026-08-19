class Grid 
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Organism[,] Cells { get; private set; }

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new Organism[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells[x, y] = new Organism(50, 100);
            }
        }
    }

    public Organism GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException("Cell coordinates are out of bounds.");

        return Cells[x, y];
    }
}
