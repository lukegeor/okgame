using System.Collections.Generic;

namespace okgame.Server.Models
{
    public interface IGame
    {
        ICollection<Player> Players { get; }
        GameState GameState { get; set; }

        void AddTile(Player player, Coordinate coordinate);
        void MoveTile(Player player, IPlayedTile playedTile, Coordinate newCoordinate);
    }
}