using System.Threading.Tasks.Dataflow;
using System.Linq;
using Xunit;
using okgame.Server.Models;
using System.Collections.Generic;
using System;

namespace okgame.Server.Tests
{
    public class GameTests
    {
        private const int NumPlayers = 4;

        private readonly Game _game;
        private readonly List<User> _users;

        public GameTests()
        {
            _users = Enumerable.Range(0, 4).Select(i => new User()).ToList();
            _game = new Game(_users);
        }

        #region CheckForWinner
        [Theory]
        [ClassData(typeof(ModelsTests_CheckForWinnerData))]
        public void Game_CheckForWinner_WinnerExists(int[,] board, int expectedWinner)
        {
            var players = _game.Players.ToList();
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] >= 0)
                    {
                        _game.GameState.PlayedTiles.Add(new PlayedTile(x, y, players[board[x, y]]));
                    }
                }
            }
            _game.GameState.MinX = 0;
            _game.GameState.MaxX = board.GetLength(0) - 1;
            _game.GameState.MinY = 0;
            _game.GameState.MaxY = board.GetLength(1) - 1;
            var winner = _game.CheckForWinner();

            Assert.Equal(expectedWinner, winner.PlayOrder);
        }
        #endregion
    }
}