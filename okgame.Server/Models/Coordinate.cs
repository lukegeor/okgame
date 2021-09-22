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
    }
}