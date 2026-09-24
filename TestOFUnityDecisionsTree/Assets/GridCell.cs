using System.Collections.Generic;

internal class GridCell
{
    public List<Pattern> possiblePatterns = new();
    public int x;
    public int y;

    public GridCell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}