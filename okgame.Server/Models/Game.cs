using System.Collections.Generic;
using System.Linq;
using okgame.Server.Exceptions;
using Microsoft.Extensions.Logging;

namespace okgame.Server.Models
{

    public class Game : IGame
    {
        private const int TilesInARowToWin = 5;
        private readonly IWinChecker _winChecker;

        public ICollection<Player> Players { get; }
        public GameState GameState { get; set; }

        private readonly ILogger _logger;

        public Game(ICollection<Player> players, IWinChecker winChecker, ILogger<Game> logger)
        {
            Players = players;
            _logger = logger;
            _winChecker = winChecker;
            GameState = new GameState();
        }

        public void AddTile(Player player, Coordinate coordinate)
        {
            CheckGameNotFinished();
            CheckRightPlayersTurn(player.PlayOrder);
            CheckAtLeastOneTileLeft(player);

            // First move of the game?
            if (GameState.PlayedTiles.Any())
            {
                // Not first move of the game
                CheckSpaceIsntOccupied(coordinate);
                CheckLegalPlacement(coordinate);
            }
            else
            {
                // First move of the game
                CheckFirstMoveLegalPlacement(coordinate);
            }

            // Placement is legal
            player.TilesLeft--;
            GameState.PlayedTiles.Add(new PlayedTile(coordinate, player.PlayOrder));

            ResetMinMax();
            IncrementPlayer();
        }

        public void MoveTile(Player player, IPlayedTile playedTile, Coordinate newCoordinate)
        {
            CheckGameNotFinished();
            CheckRightPlayersTurn(playedTile.PlayerPlayOrder);
            CheckNoTilesLeft(player);
            CheckMoveIsntToSameSpace(playedTile, newCoordinate);
            CheckSpaceIsntOccupied(newCoordinate);
            CheckLegalPlacement(newCoordinate);

            // Move is legal
            playedTile.MoveCoordinate(newCoordinate);

            ResetMinMax();
            IncrementPlayer();
        }

        private void IncrementPlayer()
        {
            GameState.Turn = (GameState.Turn + 1) % Players.Count;
        }

        private static void CheckMoveIsntToSameSpace(IPlayedTile playedTile, Coordinate coordinate)
        {
            if (playedTile.Coordinate.X == coordinate.X && playedTile.Coordinate.Y == coordinate.Y)
            {
                throw new MoveTileToSameSpaceException();
            }
        }

        private static void CheckNoTilesLeft(Player player)
        {
            if (player.TilesLeft > 0)
            {
                throw new UnplayedTilesLeftException();
            }
        }

        private static void CheckFirstMoveLegalPlacement(Coordinate coordinate)
        {
            if (coordinate.X != 0 || coordinate.Y != 0)
            {
                throw new IllegalTilePlacementException();
            }
        }

        private void CheckLegalPlacement(Coordinate coordinate)
        {
            if (!GameState.PlayedTiles.Any(pt =>
                (pt.Coordinate.X == coordinate.X && pt.Coordinate.Y == coordinate.Y + 1) ||
                (pt.Coordinate.X == coordinate.X && pt.Coordinate.Y == coordinate.Y - 1) ||
                (pt.Coordinate.Y == coordinate.Y && pt.Coordinate.X == coordinate.X + 1) ||
                (pt.Coordinate.Y == coordinate.Y && pt.Coordinate.X == coordinate.X - 1)))
            {
                throw new IllegalTilePlacementException();
            }
        }

        private void CheckSpaceIsntOccupied(Coordinate coordinate)
        {
            if (GameState.PlayedTiles.Any(pt => pt.Coordinate.X == coordinate.X && pt.Coordinate.Y == coordinate.Y))
            {
                throw new SpaceOccupiedException();
            }
        }

        private static void CheckAtLeastOneTileLeft(Player player)
        {
            if (player.TilesLeft <= 0)
            {
                throw new NoTilesLeftException();
            }
        }

        private void CheckRightPlayersTurn(int playerOrder)
        {
            if (playerOrder != GameState.Turn)
            {
                throw new WrongPlayersTurnException();
            }
        }

        private void CheckGameNotFinished()
        {
            if (GameState.Winner != null)
            {
                throw new GameAlreadyFinishedException();
            }
        }

        private void ResetMinMax()
        {
            GameState.MinX = GameState.PlayedTiles.Min(pt => pt.Coordinate.X);
            GameState.MaxX = GameState.PlayedTiles.Max(pt => pt.Coordinate.X);
            GameState.MinY = GameState.PlayedTiles.Min(pt => pt.Coordinate.Y);
            GameState.MaxY = GameState.PlayedTiles.Max(pt => pt.Coordinate.Y);
        }
    }
}