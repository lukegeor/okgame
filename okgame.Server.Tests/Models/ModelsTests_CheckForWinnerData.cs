using System.Collections.Generic;
using System.Collections;

namespace okgame.Server.Tests
{
    public class ModelsTests_CheckForWinnerData : IEnumerable<object[]>
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
                0
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
                3
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
                1
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
                0
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
                3
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
                1
            };
            #endregion Tall Board Horizontal Winner
        }
    }
}