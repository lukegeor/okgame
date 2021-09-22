using System.Collections.Generic;
using System.Linq;
using okgame.Server.Exceptions;
using System;
using Microsoft.Extensions.Logging;

namespace okgame.Server.Models
{
    public class Game : IGame
    {
        private const int TilesInARowToWin = 5;

        public IEnumerable<Player> Players { get; }
        public GameState GameState { get; set; }

        private readonly ILogger _logger;

        public Game(IEnumerable<Player> players, ILogger<Game> logger)
        {
            Players = players;
            _logger = logger;
            GameState = new GameState(Players.First());
        }

        public void AddTile(Player player, Coordinate coordinate)
        {
            CheckGameNotFinished();
            CheckRightPlayersTurn(player);
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
            GameState.PlayedTiles.Add(new PlayedTile(coordinate, player));

            ResetMinMax();
            IncrementPlayer();
        }

        public void MoveTile(PlayedTile playedTile, Coordinate newCoordinate)
        {
            CheckGameNotFinished();
            CheckRightPlayersTurn(playedTile.Player);
            CheckNoTilesLeft(playedTile);
            CheckMoveIsntToSameSpace(playedTile, newCoordinate);
            CheckSpaceIsntOccupied(newCoordinate);
            CheckLegalPlacement(newCoordinate);

            // Move is legal
            playedTile.MoveCoordinate(newCoordinate);

            ResetMinMax();
            IncrementPlayer();
        }

        public Player? CheckForWinner()
        {
            var directionLambdasList = new[]
            {
                // Vertical
                new
                {
                    Description = "Vertical",
                    MajorVar = "Y",
                    MinorVar = "X",
                    MinMajor = GameState.MinY,
                    MaxMajor = GameState.MaxY,
                    MinMinor = GameState.MinX,
                    MaxMinor = GameState.MaxX,
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Coordinate.Y),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.Coordinate.X),
                    AdjustMajorForDiagonal = (Func<int, int, int>) ((originalMinor, diagonalOffset) => originalMinor)
                },
                // Horizontal
                new
                {
                    Description = "Horizontal",
                    MajorVar = "X",
                    MinorVar = "Y",
                    MinMajor = GameState.MinX,
                    MaxMajor = GameState.MaxX,
                    MinMinor = GameState.MinY,
                    MaxMinor = GameState.MaxY,
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Coordinate.X),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.Coordinate.Y),
                    AdjustMajorForDiagonal = (Func<int, int, int>) ((originalMinor, diagonalOffset) => originalMinor)
                },
                // Diagonal
                new
                {
                    Description = "Diagonal down/right",
                    MajorVar = "Y",
                    MinorVar = "X",
                    MinMajor = GameState.MinY - (GameState.MaxX - GameState.MinX),
                    MaxMajor = GameState.MaxY,
                    MinMinor = GameState.MinX,
                    MaxMinor = GameState.MaxX,
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Coordinate.Y),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.Coordinate.X),
                    AdjustMajorForDiagonal = (Func<int, int, int>) ((originalMinor, diagonalOffset) => originalMinor + diagonalOffset)
                },
                // Diagonal
                new
                {
                    Description = "Diagonal down/left",
                    MajorVar = "Y",
                    MinorVar = "X",
                    MinMajor = GameState.MinY,
                    MaxMajor = GameState.MaxY + (GameState.MaxX - GameState.MinX),
                    MinMinor = GameState.MinX,
                    MaxMinor = GameState.MaxX,
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Coordinate.Y),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.Coordinate.X),
                    AdjustMajorForDiagonal = (Func<int, int, int>) ((originalMinor, diagonalOffset) => originalMinor - diagonalOffset)
                },
            };
            foreach (var directionLambdas in directionLambdasList)
            {
                _logger.LogDebug($"---- {directionLambdas.Description} ----");
                Player? playerTest;
                int tilesInARow = 0;
                for (int major = directionLambdas.MinMajor; major <= directionLambdas.MaxMajor; major++)
                {
                    _logger.LogDebug($"**** Major {directionLambdas.MajorVar} {major}");
                    playerTest = null;
                    tilesInARow = 0;
                    for (int minor = directionLambdas.MinMinor, i = 0; minor <= directionLambdas.MaxMinor; minor++, i++)
                    {
                        var adjustedMajor = directionLambdas.AdjustMajorForDiagonal(major, i);
                        _logger.LogDebug($"  ** Minor {directionLambdas.MinorVar} {minor}");
                        _logger.LogDebug($"  ** Tile At {directionLambdas.MajorVar} {adjustedMajor}, {directionLambdas.MinorVar} {minor}");
                        PlayedTile? currentPlayedTile = GameState.PlayedTiles.SingleOrDefault(pt => directionLambdas.GetMinor(pt) == minor && directionLambdas.GetMajor(pt) == adjustedMajor);
                        _logger.LogDebug($"     Tile owned by {currentPlayedTile?.Player.ToString() ?? "(none)"}");
                        if (currentPlayedTile == null)
                        {
                            playerTest = null;
                            tilesInARow = 0;
                        }
                        else
                        {
                            if (currentPlayedTile.Player == playerTest)
                            {
                                tilesInARow++;
                            }
                            else
                            {
                                playerTest = currentPlayedTile.Player;
                                tilesInARow = 1;
                            }
                        }
                        _logger.LogDebug($"      playerTest = {playerTest?.ToString() ?? "(none)"}, tilesInARow = {tilesInARow}");

                        if (tilesInARow >= TilesInARowToWin)
                        {
                            GameState.Winner = playerTest!;
                            return playerTest!;
                        }
                    }
                }
            }

            return null;
        }

        private Player IncrementPlayer()
        {
            var currentPlayerOrder = GameState.Turn.PlayOrder;
            var nextPlayer = Players.OrderBy(p => p.PlayOrder).FirstOrDefault(p => p.PlayOrder > currentPlayerOrder);
            if (nextPlayer == null)
            {
                nextPlayer = Players.OrderBy(p => p.PlayOrder).First();
            }
            GameState.Turn = nextPlayer!;
            return nextPlayer;
        }

        private static void CheckMoveIsntToSameSpace(PlayedTile playedTile, Coordinate coordinate)
        {
            if (playedTile.Coordinate.X == coordinate.X && playedTile.Coordinate.Y == coordinate.Y)
            {
                throw new MoveTileToSameSpaceException();
            }
        }

        private static void CheckNoTilesLeft(PlayedTile playedTile)
        {
            if (playedTile.Player.TilesLeft > 0)
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

        private void CheckRightPlayersTurn(Player player)
        {
            if (player != GameState.Turn)
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