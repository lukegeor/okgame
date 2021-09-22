using okgame.Server.Models;

using AutoFixture;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using AutoFixture.AutoMoq;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using okgame.Server.Tests.TestUtils;
using System;

namespace okgame.Server.Tests
{
    public class GameTests
    {
        private const int NumPlayers = 4;

        private readonly IFixture _fixture;
        private readonly Game _game;
        private readonly IEnumerable<Player> _players;

        public GameTests(ITestOutputHelper outputHelper)
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _players = Enumerable.Range(0, NumPlayers).Select(i => new User()).Select((u, i) => new Player(u, i));
            _fixture.Inject<IEnumerable<Player>>(_players);
            _fixture.Inject<ILogger<Game>>(new XunitLogger<Game>(outputHelper));
            _game = _fixture.Create<Game>();
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
                        _game.GameState.PlayedTiles.Add(new PlayedTile(new Coordinate(x, y), players[board[x, y]]));
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
 
        #region CheckForWinner2
        [Theory]
        [ClassData(typeof(ModelsTests_CheckForWinner2Data))]
        public void Game_CheckForWinner2_WinnerExistsUseFirst(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates)
        {
            var justPlayedCoordinate = expectedWinningCoordinates.First();
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        [Theory]
        [ClassData(typeof(ModelsTests_CheckForWinner2Data))]
        public void Game_CheckForWinner2_WinnerExistsUseLast(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates)
        {
            var justPlayedCoordinate = expectedWinningCoordinates.Last();
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        [Theory]
        [ClassData(typeof(ModelsTests_CheckForWinner2Data))]
        public void Game_CheckForWinner2_WinnerExistsUseMiddle(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates)
        {
            var justPlayedCoordinate = expectedWinningCoordinates.ElementAt(expectedWinningCoordinates.Count() / 2);
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        [Theory]
        [ClassData(typeof(ModelsTests_CheckForWinner2TwoWayData))]
        public void Game_CheckForWinner2_TwoWayWinnerExists(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates, Coordinate justPlayedCoordinate)
        {
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        private void CheckWinnerExistsUsingSpecifiedPlayedCoordinate(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates, Coordinate justPlayedCoordinate)
        {
            var players = _game.Players.ToList();
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] >= 0)
                    {
                        _game.GameState.PlayedTiles.Add(new PlayedTile(new Coordinate(x, y), players[board[x, y]]));
                    }
                }
            }
            _game.GameState.MinX = 0;
            _game.GameState.MaxX = board.GetLength(0) - 1;
            _game.GameState.MinY = 0;
            _game.GameState.MaxY = board.GetLength(1) - 1;
            var justPlayedTile = _game.GameState.PlayedTiles.First(playedTile => playedTile.Coordinate.Equals(justPlayedCoordinate));
            var computedWinningCoordinates = _game.CheckForWinner2(justPlayedTile);

            Assert.True(expectedWinningCoordinates.OrderBy(c => c.X).ThenBy(c => c.Y).SequenceEqual(computedWinningCoordinates.OrderBy(c => c.X).ThenBy(c => c.Y)));
        }
        #endregion
    }
}