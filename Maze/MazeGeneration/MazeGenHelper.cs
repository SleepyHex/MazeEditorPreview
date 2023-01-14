using System.Collections.Generic;
using UnityEngine;

namespace MazeGen
{
    public static class MazeGenHelper
    {
        public static WallState[,] RemoveWallsBetweenCurrentAndNext(WallState[,] maze, Neighbor nextNeighbor, Vector2Int current, Vector2Int next)
        {
            maze[current.x, current.y] &= ~nextNeighbor.SharedWall;//removes shared walls from current position
            maze[next.x, next.y] &= ~GetOppositeWall(nextNeighbor.SharedWall);

            return maze;
        }

        public static WallState GetOppositeWall(WallState wall)
        {
            switch(wall)
            {
                case WallState.RIGHT: return WallState.LEFT;
                case WallState.LEFT: return WallState.RIGHT;
                case WallState.DOWN: return WallState.UP;
                case WallState.UP: return WallState.DOWN;
                default: return WallState.LEFT;
            }
        }

        public static bool DetermineIfGridContainsVisited(MazeWallData wallData)
        {
            for(int i = 0; i < wallData.Width; i++)
                for(int j = 0; j < wallData.Height; j++)
                    if(wallData.IsVisited(i, j))
                        return true;
            return false;
        }
        public static bool DetermineIfGridContainsUnvisited(MazeWallData wallData) => !DetermineIfGridContainsVisited(wallData);

        public static List<Neighbor> GetUnvisitedNeighbors(Vector2Int pos, MazeWallData wallData)
        {
            List<Neighbor> neighbors = new List<Neighbor>();

            AddNeighborIf(pos.x > 0, WallState.LEFT, new Vector2Int(pos.x - 1, pos.y), neighbors, wallData);
            AddNeighborIf(pos.y > 0, WallState.DOWN, new Vector2Int(pos.x, pos.y - 1), neighbors, wallData);
            AddNeighborIf(pos.y < wallData.Height - 1, WallState.UP, new Vector2Int(pos.x, pos.y + 1), neighbors, wallData);
            AddNeighborIf(pos.x < wallData.Width - 1, WallState.RIGHT, new Vector2Int(pos.x + 1, pos.y), neighbors, wallData);

            return neighbors;
        }

        public static List<Neighbor> GetVisitedNeighbors(Vector2Int pos, MazeWallData wallData)
        {
            List<Neighbor> neighbors = new List<Neighbor>();

            AddNeighborIf(pos.x > 0, WallState.LEFT, new Vector2Int(pos.x - 1, pos.y), neighbors, wallData, visited: true);
            AddNeighborIf(pos.y > 0, WallState.DOWN, new Vector2Int(pos.x, pos.y - 1), neighbors, wallData, visited: true);
            AddNeighborIf(pos.y < wallData.Height - 1, WallState.UP, new Vector2Int(pos.x, pos.y + 1), neighbors, wallData, visited: true);
            AddNeighborIf(pos.x < wallData.Width - 1, WallState.RIGHT, new Vector2Int(pos.x + 1, pos.y), neighbors, wallData, visited: true);

            return neighbors;
        }

        private static void AddNeighborIf(bool condition, WallState direction, Vector2Int pos, List<Neighbor> neighbors, MazeWallData wallData, bool visited = false)
        {
            if(condition)
            {
                bool isVisited = wallData.Walls[pos.x, pos.y].HasFlag(WallState.VISITED);
                if(visited ? isVisited : !isVisited)
                    neighbors.Add(new Neighbor(pos, direction));
            }
        }
    }
}