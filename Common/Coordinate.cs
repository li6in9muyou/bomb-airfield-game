namespace Common;

public class Coordinate
{
    public int X;
    public int Y;

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Coordinate Void()
    {
        return new Coordinate(-1, -1);
    }

    public override string ToString()
    {
        return $"Coordinate({nameof(X)}: {X}, {nameof(Y)}: {Y})";
    }

    private bool Equals(Coordinate other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Coordinate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}