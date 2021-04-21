namespace okgame.Server.Models
{
    public class PlayedTile
    {
        // Vertical
        public int X { get; set; }

        // Horizontal
        public int Y { get; set; }

        public Player Player { get; set; }

        public PlayedTile(int x, int y, Player player)
        {
            X = x;
            Y = y;
            Player = player;
        }
    }
}