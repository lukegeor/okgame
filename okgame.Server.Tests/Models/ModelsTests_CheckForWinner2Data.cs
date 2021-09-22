using System.Collections.Generic;
using System.Collections;
using okgame.Server.Models;

namespace okgame.Server.Tests
{
    public class ModelsTests_CheckForWinner2Data : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Tall Board Vertical Winner
            yield return new object[]
            {
                new int[,] {
                    {0, 1, 2, 3},
                    {0, 2, 3, 1},
                    {0, 1, 2, 3},
                    {0, 2, 3, 1},
                    {0, 1, 2, 3}
                },
                new Coordinate[] {
                    new Coordinate(0, 0),
                    new Coordinate(1, 0),
                    new Coordinate(2, 0),
                    new Coordinate(3, 0),
                    new Coordinate(4, 0),
                }
            };
            yield return new object[]
            {
                new int[,] {
                    {1, 3, 2, 0},
                    {2, 3, 0, 1},
                    {1, 3, 2, 0},
                    {2, 3, 0, 1},
                    {1, 3, 2, 0}
                },
                new Coordinate[] {
                    new Coordinate(0, 1),
                    new Coordinate(1, 1),
                    new Coordinate(2, 1),
                    new Coordinate(3, 1),
                    new Coordinate(4, 1),
                }
            };
            yield return new object[]
            {
                new int[,] {
                    {0, 3, 2, 1},
                    {2, 0, 3, 1},
                    {0, 3, 2, 1},
                    {2, 0, 3, 1},
                    {0, 3, 2, 1}
                },
                new Coordinate[] {
                    new Coordinate(0, 3),
                    new Coordinate(1, 3),
                    new Coordinate(2, 3),
                    new Coordinate(3, 3),
                    new Coordinate(4, 3),
                }
            };
            #endregion Tall Board Vertical Winner
            #region Tall Board Horizontal Winner
            yield return new object[]
            {
                new int[,] {
                    {1, 0, 0, 0, 0, 0},
                    {2, 1, 1, 2, 3, 1},
                    {3, 2, 2, 1, 0, 3},
                    {0, 3, 3, 1, 3, 2},
                    {1, 0, 2, 3, 2, 3},
                    {1, 2, 2, 1, 0, 3},
                    {0, 3, 3, 1, 3, 2},
                },
                new Coordinate[] {
                    new Coordinate(0, 1),
                    new Coordinate(0, 2),
                    new Coordinate(0, 3),
                    new Coordinate(0, 4),
                    new Coordinate(0, 5),
                }
            };
            yield return new object[]
            {
                new int[,] {
                    {1, 0, 0, 3, 0, 0},
                    {2, 1, 1, 2, 3, 1},
                    {3, 2, 2, 1, 0, 3},
                    {3, 3, 3, 3, 3, 2},
                    {1, 0, 2, 3, 2, 3},
                    {1, 2, 2, 1, 0, 3},
                    {0, 3, 3, 1, 3, 2},
                },
                new Coordinate[] {
                    new Coordinate(3, 0),
                    new Coordinate(3, 1),
                    new Coordinate(3, 2),
                    new Coordinate(3, 3),
                    new Coordinate(3, 4),
                }
            };
            yield return new object[]
            {
                new int[,] {
                    {1, 0, 0, 3, 0, 0},
                    {2, 1, 1, 2, 3, 1},
                    {3, 2, 2, 1, 0, 3},
                    {3, 3, 2, 3, 3, 2},
                    {1, 0, 2, 3, 2, 3},
                    {1, 2, 2, 1, 0, 3},
                    {1, 1, 1, 1, 1, 2},
                },
                new Coordinate[] {
                    new Coordinate(6, 0),
                    new Coordinate(6, 1),
                    new Coordinate(6, 2),
                    new Coordinate(6, 3),
                    new Coordinate(6, 4),
                }
            };
            #endregion Tall Board Horizontal Winner
            #region Right Diagonal Winner
            yield return new object[]
            {
                new int[,] {
                    {1, 3, 2, 0, 2, 2},
                    {2, 1, 1, 2, 3, 1},
                    {3, 2, 1, 1, 0, 3},
                    {0, 3, 3, 1, 3, 2},
                    {1, 0, 2, 3, 1, 3},
                    {1, 2, 2, 1, 0, 3},
                    {0, 3, 3, 2, 3, 2},
                },
                new Coordinate[] {
                    new Coordinate(0, 0),
                    new Coordinate(1, 1),
                    new Coordinate(2, 2),
                    new Coordinate(3, 3),
                    new Coordinate(4, 4),
                }
            };
            yield return new object[]
            {
                new int[,] {
                    {1, 0, 0, 3, 0, 3},
                    {2, 3, 1, 2, 1, 1},
                    {2, 2, 3, 1, 0, 3},
                    {3, 0, 3, 3, 3, 2},
                    {1, 0, 2, 2, 3, 3},
                    {1, 2, 2, 1, 0, 3},
                    {0, 3, 3, 1, 3, 2},
                },
                new Coordinate[] {
                    new Coordinate(1, 1),
                    new Coordinate(2, 2),
                    new Coordinate(3, 3),
                    new Coordinate(4, 4),
                    new Coordinate(5, 5),
                }
            };
            yield return new object[]
            {
                new int[,] {
                    {1, 0, 0, 3, 0, 1},
                    {2, 1, 1, 2, 3, 1},
                    {3, 2, 2, 1, 0, 3},
                    {3, 3, 2, 3, 3, 2},
                    {1, 0, 2, 2, 3, 3},
                    {1, 2, 2, 1, 2, 3},
                    {1, 1, 0, 1, 1, 0},
                },
                new Coordinate[] {
                    new Coordinate(1, 0),
                    new Coordinate(2, 1),
                    new Coordinate(3, 2),
                    new Coordinate(4, 3),
                    new Coordinate(5, 4),
                }
            };
            #endregion Right Diagonal Winner
        }
    }
}