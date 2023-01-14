using System.Collections.Generic;
using MazeAlgos;
using UnityEngine;
using static MazeAlgos.MazeAlgorithmChoices;

namespace MazeGen
{
    public class MazeWallGenerator : MonoBehaviour
    {
        public static class MazeGenerator
        {
            static int Seed;
            static Dictionary<AlgorithmChoice, IMazeAlgorithm> Algorithms { get; }

            static MazeGenerator() => Algorithms = new Dictionary<AlgorithmChoice, IMazeAlgorithm>
            {
                { AlgorithmChoice.RandomDFS, new MazeAlgoRandomDFS() },
                { AlgorithmChoice.HuntAndKill, new MazeAlgoHuntAndKill() },
                { AlgorithmChoice.Wilsons, new MazeAlgoWilsons() },
                { AlgorithmChoice.Aldous_Broder, new MazeAlgoAldous_Broder() }
            };

            public static MazeWallData Generate(AlgorithmChoice algoChoice, Vector2Int size, int seed)
            {
                Seed = seed;
                MazeWallData maze = new (size.x, size.y);
                WallState defaultState = WallState.LEFT | WallState.RIGHT | WallState.UP | WallState.DOWN;

                for(int i = 0; i < size.x; i++)
                    for(int j = 0; j < size.y; j++)
                        maze.Walls[i, j] = defaultState;

                return Algorithms[algoChoice].ExecuteAlgorithm(maze, seed);
            }
        }
    }
}