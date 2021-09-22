using System.Collections.Generic;

namespace okgame.Server.Models
{
    public interface IWinChecker
    {
        (int WinnerPlayOrder, IEnumerable<Coordinate> WinningTileCoordinates)? CheckForWinner(GameState gameState, IPlayedTile justPlayedTile);
    }
}