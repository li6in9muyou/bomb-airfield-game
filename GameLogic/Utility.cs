namespace GameLogic
{
    public class Coordinate
    {
        public int X;
        public int Y;

        Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        static Coordinate Void ()
        {
            return new Coordinate(-1, -1);
        }
    }
}