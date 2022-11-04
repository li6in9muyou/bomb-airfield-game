namespace Database;

public class Airplane
{
    public Airplane(int x1, int y1, string direction1)
    {
        X = x1;
        Y = y1;
        Direction = direction1;
    }

    public Airplane()
    {
    }

    //up down left right
    public int X { get; set; }

    public int Y { get; set; }

    public string Direction { get; set; }
}