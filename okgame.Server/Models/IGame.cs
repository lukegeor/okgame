using System.Collections.Generic;

namespace okgame.Server.Models
{
    public interface IGame
    {
        IEnumerable<Player> Players { get; }
        GameState GameState { get; set; }

        void AddTile(Player player, Coordinate coordinate);
        Player? CheckForWinner();
        void MoveTile(PlayedTile playedTile, Coordinate newCoordinate);
    }
}