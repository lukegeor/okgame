using AutoFixture;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using AutoFixture.AutoMoq;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using okgame.Server.Tests.TestUtils;
using System;

namespace okgame.Server.Models.Tests
{
    public class PlayedTileWinCheckerTests
    {
        private const int NumPlayers = 4;

        private readonly IFixture _fixture;
        private readonly PlayedTileWinChecker _winChecker;

        public PlayedTileWinCheckerTests(ITestOutputHelper outputHelper)
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Inject<int>(5);
            _fixture.Inject<ILogger<PlayedTileWinChecker>>(new XunitLogger<PlayedTileWinChecker>(outputHelper));
            _winChecker = _fixture.Create<PlayedTileWinChecker>();
        }

        #region CheckForWinner
        [Theory]
        [ClassData(typeof(PlayedTileWinCheckerTests_CheckForWinnerData))]
        public void Game_CheckForWinner_WinnerExistsUseFirst(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates)
        {
            var justPlayedCoordinate = expectedWinningCoordinates.First();
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        [Theory]
        [ClassData(typeof(PlayedTileWinCheckerTests_CheckForWinnerData))]
        public void Game_CheckForWinner_WinnerExistsUseLast(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates)
        {
            var justPlayedCoordinate = expectedWinningCoordinates.Last();
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        [Theory]
        [ClassData(typeof(PlayedTileWinCheckerTests_CheckForWinnerData))]
        public void Game_CheckForWinner_WinnerExistsUseMiddle(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates)
        {
            var justPlayedCoordinate = expectedWinningCoordinates.ElementAt(expectedWinningCoordinates.Count() / 2);
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        [Theory]
        [ClassData(typeof(PlayedTileWinCheckerTests_CheckForWinnerTwoWayData))]
        public void Game_CheckForWinner_TwoWayWinnerExists(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates, Coordinate justPlayedCoordinate)
        {
            CheckWinnerExistsUsingSpecifiedPlayedCoordinate(board, expectedWinningCoordinates, justPlayedCoordinate);
        }

        private void CheckWinnerExistsUsingSpecifiedPlayedCoordinate(int[,] board, IEnumerable<Coordinate> expectedWinningCoordinates, Coordinate justPlayedCoordinate)
        {
            GameState gameState = new GameState();
            //var players = _winChecker.Players.ToList();
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] >= 0)
                    {
                        gameState.PlayedTiles.Add(new PlayedTile(new Coordinate(x, y), board[x, y]));
                    }
                }
            }
            gameState.MinX = 0;
            gameState.MaxX = board.GetLength(0) - 1;
            gameState.MinY = 0;
            gameState.MaxY = board.GetLength(1) - 1;
            var justPlayedTile = gameState.PlayedTiles.First(playedTile => playedTile.Coordinate.Equals(justPlayedCoordinate));
            var computedWinningCoordinates = _winChecker.CheckForWinner(gameState, justPlayedTile);

            Assert.NotNull(computedWinningCoordinates);
            Assert.Equal(board[justPlayedCoordinate.X, justPlayedCoordinate.Y], computedWinningCoordinates.Value.WinnerPlayOrder);
            Assert.True(expectedWinningCoordinates.OrderBy(c => c.X).ThenBy(c => c.Y).SequenceEqual(computedWinningCoordinates.Value.WinningTileCoordinates.OrderBy(c => c.X).ThenBy(c => c.Y)));
        }
        #endregion
    }
}