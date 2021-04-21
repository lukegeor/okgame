using System.Collections.Generic;

namespace okgame.Server.Models
{
    public class GameState
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public ICollection<PlayedTile> PlayedTiles { get; }

        public Player? Winner { get; set; }

        public Player Turn { get; set; }

        public GameState(Player startingPlayer)
        {
            PlayedTiles = new List<PlayedTile>();
            Winner = null;
            Turn = startingPlayer;
            MinX = MaxY = MinY = MaxY = 0;
        }
    }
}