// třída cell slouží jako wrapper pro snažší orientaci v gridu
// grid má pole Cells o velikosti X x Y a v každé buňce pole je jedna Cell
// Cell si pamatuje, na jakých souřadnicích se nachází, který organismus a živiny (konkrétní instance) 
// se na ní zrovna nachází 
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