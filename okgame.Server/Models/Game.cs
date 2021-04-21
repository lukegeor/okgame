using System.Collections.Generic;
using System.Linq;
using okgame.Server.Exceptions;
using System;

namespace okgame.Server.Models
{
    public class Game
    {
        public const int TilesInARowToWin = 5;

        public IEnumerable<Player> Players { get; }
        public GameState GameState { get; set; }

        public Game(IEnumerable<User> users)
        {
            Players = users.Select((u, i) => new Player(u, i));
            GameState = new GameState(Players.First());
        }

        public void AddTile(Player player, int x, int y)
        {
            CheckGameNotFinished();
            CheckRightPlayersTurn(player);
            CheckAtLeastOneTileLeft(player);

            // First move of the game?
            if (GameState.PlayedTiles.Any())
            {
                // Not first move of the game
                CheckSpaceIsntOccupied(x, y);
                CheckLegalPlacement(x, y);
            }
            else
            {
                // First move of the game
                CheckFirstMoveLegalPlacement(x, y);
            }

            // Placement is legal
            player.TilesLeft--;
            GameState.PlayedTiles.Add(new PlayedTile(x, y, player));

            ResetMinMax();
            IncrementPlayer();
        }

        public void MoveTile(PlayedTile playedTile, int newX, int newY)
        {
            CheckGameNotFinished();
            CheckRightPlayersTurn(playedTile.Player);
            CheckNoTilesLeft(playedTile);
            CheckMoveIsntToSameSpace(playedTile, newX, newY);
            CheckSpaceIsntOccupied(newX, newY);
            CheckLegalPlacement(newX, newY);

            // Move is legal
            playedTile.X = newX;
            playedTile.Y = newY;

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
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Y),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.X),
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
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.X),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.Y),
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
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Y),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.X),
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
                    GetMajor = (Func<PlayedTile, int>) (pt => pt.Y),
                    GetMinor = (Func<PlayedTile, int>) (pt => pt.X),
                    AdjustMajorForDiagonal = (Func<int, int, int>) ((originalMinor, diagonalOffset) => originalMinor - diagonalOffset)
                },
            };
            foreach (var directionLambdas in directionLambdasList)
            {
                Console.WriteLine($"---- {directionLambdas.Description} ----");
                Player? playerTest;
                int tilesInARow = 0;
                for (int major = directionLambdas.MinMajor; major <= directionLambdas.MaxMajor; major++)
                {
                    Console.WriteLine($"**** Major {directionLambdas.MajorVar} {major}");
                    playerTest = null;
                    tilesInARow = 0;
                    for (int minor = directionLambdas.MinMinor, i = 0; minor <= directionLambdas.MaxMinor; minor++, i++)
                    {
                        var adjustedMajor = directionLambdas.AdjustMajorForDiagonal(major, i);
                        Console.WriteLine($"  ** Minor {directionLambdas.MinorVar} {minor}");
                        Console.WriteLine($"  ** Tile At {directionLambdas.MajorVar} {adjustedMajor}, {directionLambdas.MinorVar} {minor}");
                        PlayedTile? currentPlayedTile = GameState.PlayedTiles.SingleOrDefault(pt => directionLambdas.GetMinor(pt) == minor && directionLambdas.GetMajor(pt) == adjustedMajor);
                        Console.WriteLine($"     Tile owned by {currentPlayedTile?.Player.ToString() ?? "(none)"}");
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
                        Console.WriteLine($"      playerTest = {playerTest?.ToString() ?? "(none)"}, tilesInARow = {tilesInARow}");

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

        private static void CheckMoveIsntToSameSpace(PlayedTile playedTile, int x, int y)
        {
            if (playedTile.X == x && playedTile.Y == y)
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

        private static void CheckFirstMoveLegalPlacement(int x, int y)
        {
            if (x != 0 || y != 0)
            {
                throw new IllegalTilePlacementException();
            }
        }

        private void CheckLegalPlacement(int x, int y)
        {
            if (!GameState.PlayedTiles.Any(pt =>
                (pt.X == x && pt.Y == y + 1) ||
                (pt.X == x && pt.Y == y - 1) ||
                (pt.Y == y && pt.X == x + 1) ||
                (pt.Y == y && pt.X == x - 1)))
            {
                throw new IllegalTilePlacementException();
            }
        }

        private void CheckSpaceIsntOccupied(int x, int y)
        {
            if (GameState.PlayedTiles.Any(pt => pt.X == x && pt.Y == y))
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
            GameState.MinX = GameState.PlayedTiles.Min(pt => pt.X);
            GameState.MaxX = GameState.PlayedTiles.Max(pt => pt.X);
            GameState.MinY = GameState.PlayedTiles.Min(pt => pt.Y);
            GameState.MaxY = GameState.PlayedTiles.Max(pt => pt.Y);
        }
    }
}