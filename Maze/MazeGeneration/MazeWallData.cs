using UnityEngine;

namespace MazeGen
{
    public class MazeWallData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public WallState[,] Walls { get; set; }

        public MazeWallData(int width, int height)
        {
            Width = width;
            Height = height;
            Walls = new WallState[Width, Height];
        }

        public void SetVisited(int x, int y) => Walls[x, y] |= WallState.VISITED;
        public void SetVisited(Vector2Int size) => SetVisited(size.x, size.y);

        public void ClearVisited(int x, int y) => Walls[x, y] &= ~WallState.VISITED;
        public void ClearVisited(Vector2Int size) => ClearVisited(size.x, size.y);

        public bool IsVisited(int x, int y) => Walls[x, y].HasFlag(WallState.VISITED);
        public bool IsVisited(Vector2Int size) => IsVisited(size.x, size.y);

        public void ClearWallsBetweenCurrentAndNext(Neighbor nextNeighbor, Vector2Int current, Vector2Int next) => Walls = MazeGenHelper.RemoveWallsBetweenCurrentAndNext(Walls, nextNeighbor, current, next);
    }
}