namespace Database;

public class Airplane
{

    private int x, y;
    private string direction = null;
    //up down left right
    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }
    public string Direction { get { return direction; } set { direction = value; } }

    public Airplane(int x1, int y1, string direction1)
    {
        x = x1;
        y = y1;
        direction = direction1;
    }
    public Airplane() { }
}