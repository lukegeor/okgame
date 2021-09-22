using System.Collections.Generic;
using System.Collections;
using okgame.Server.Models;

namespace okgame.Server.Tests
{
    public class ModelsTests_CheckForWinner2TwoWayData : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Two Way Winner
            yield return new object[]
            {
                new int[,] {
                    {1, 3, 2, 0, 1, 2},
                    {2, 1, 1, 2, 1, 3},
                    {3, 2, 1, 2, 1, 3},
                    {0, 3, 3, 1, 1, 2},
                    {0, 0, 2, 3, 1, 3},
                    {1, 2, 2, 3, 0, 3},
                    {0, 3, 3, 2, 3, 2},
                },
                new Coordinate[] {
                    new Coordinate(0, 0),
                    new Coordinate(1, 1),
                    new Coordinate(2, 2),
                    new Coordinate(3, 3),
                    new Coordinate(4, 4),
                    new Coordinate(3, 4),
                    new Coordinate(2, 4),
                    new Coordinate(1, 4),
                    new Coordinate(0, 4),
                },
                new Coordinate(4, 4)
            };
            #endregion Two Way Winner
        }
    }
}