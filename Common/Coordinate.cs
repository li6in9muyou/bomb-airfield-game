﻿namespace Common
{
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
    }
}