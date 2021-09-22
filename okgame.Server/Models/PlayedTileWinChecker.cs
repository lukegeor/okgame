using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace okgame.Server.Models
{
    public class PlayedTileWinChecker : IWinChecker
    {
        private readonly int _tilesInARowToWin;
        private readonly ILogger<PlayedTileWinChecker> _logger;

        public PlayedTileWinChecker(int tilesInARowToWin, ILogger<PlayedTileWinChecker> logger)
        {
            _tilesInARowToWin = tilesInARowToWin;
            _logger = logger;
        }

        public (int WinnerPlayOrder, IEnumerable<Coordinate> WinningTileCoordinates)? CheckForWinner(GameState gameState, IPlayedTile justPlayedTile)
        {
            var directions = new (Func<Coordinate, Coordinate> NextCoordinate, Func<Coordinate, Coordinate> PreviousCoordinate)[]
            {
                // |
                (
                    coordinate => new Coordinate(coordinate.X + 1, coordinate.Y),
                    coordinate => new Coordinate(coordinate.X - 1, coordinate.Y)
                ),
                // --
                (
                    coordinate => new Coordinate(coordinate.X, coordinate.Y + 1),
                    coordinate => new Coordinate(coordinate.X, coordinate.Y - 1)
                ),
                // \
                (
                    coordinate => new Coordinate(coordinate.X + 1, coordinate.Y + 1),
                    coordinate => new Coordinate(coordinate.X - 1, coordinate.Y - 1)
                ),
                // /
                (
                    coordinate => new Coordinate(coordinate.X + 1, coordinate.Y - 1),
                    coordinate => new Coordinate(coordinate.X - 1, coordinate.Y + 1)
                ),
            };

            var allWinningTiles = new HashSet<Coordinate>();
            foreach (var direction in directions)
            {
                var thisDirectionWinningTiles = new HashSet<Coordinate>();
                thisDirectionWinningTiles.Add(justPlayedTile.Coordinate);

                var currentCoordinate = justPlayedTile.Coordinate;
                while (currentCoordinate != null)
                {
                    currentCoordinate = direction.NextCoordinate(currentCoordinate);
                    if (gameState.PlayedTiles.Any(checkTile => checkTile.Coordinate.Equals(currentCoordinate) && checkTile.PlayerPlayOrder == justPlayedTile.PlayerPlayOrder))
                    {
                        thisDirectionWinningTiles.Add(currentCoordinate);
                    }
                    else
                    {
                        currentCoordinate = null;
                    }
                }
                currentCoordinate = justPlayedTile.Coordinate;
                while (currentCoordinate != null)
                {
                    currentCoordinate = direction.PreviousCoordinate(currentCoordinate);
                    if (gameState.PlayedTiles.Any(checkTile => checkTile.Coordinate.Equals(currentCoordinate) && checkTile.PlayerPlayOrder == justPlayedTile.PlayerPlayOrder))
                    {
                        thisDirectionWinningTiles.Add(currentCoordinate);
                    }
                    else
                    {
                        currentCoordinate = null;
                    }
                }

                if (thisDirectionWinningTiles.Count >= _tilesInARowToWin)
                {
                    allWinningTiles.UnionWith(thisDirectionWinningTiles);
                }
            }

            return allWinningTiles.Any() ? (justPlayedTile.PlayerPlayOrder, allWinningTiles) : null;
        }
    }
}