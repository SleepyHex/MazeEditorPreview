using MazeGen;

namespace MazeAlgos
{
    public abstract class IMazeAlgorithm
    {
        public abstract MazeWallData ExecuteAlgorithm(MazeWallData maze, int Seed);
    }
}