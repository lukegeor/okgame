using System.Collections.Generic;

namespace okgame.Server.Models
{
    public class GameState
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public ICollection<IPlayedTile> PlayedTiles { get; }

        public int? Winner { get; set; }

        public int Turn { get; set; }

        public GameState()
        {
            PlayedTiles = new List<IPlayedTile>();
            Winner = null;
            Turn = 0;
            MinX = MaxY = MinY = MaxY = 0;
        }
    }
}