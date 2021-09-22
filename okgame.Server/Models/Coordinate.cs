using System;

namespace okgame.Server.Models
{
    public class Coordinate
    {
        // Vertical
        public int X { get; }

        // Horizontal
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coordinate coordinate &&
                   X == coordinate.X &&
                   Y == coordinate.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString() => $"({X},{Y})";
    }
}