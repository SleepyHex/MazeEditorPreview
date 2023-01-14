using System.Collections.Generic;
using MazeGen;
using UnityEngine;
using static MazeGen.MazeGenHelper;
using Random = System.Random;

namespace MazeAlgos
{
    public class MazeAlgoAldous_Broder : IMazeAlgorithm
    {
        public override MazeWallData ExecuteAlgorithm(MazeWallData wallData, int seed)
        {
            Random rng = new Random(seed);

            // initialize all cells to have all walls
            for(int i = 0; i < wallData.Width; i++)
            {
                for(int j = 0; j < wallData.Height; j++)
                {
                    wallData.Walls[i, j] = WallState.LEFT | WallState.RIGHT | WallState.UP | WallState.DOWN;
                }
            }

            // choose a random starting cell
            int currentX = rng.Next(0, wallData.Width);
            int currentY = rng.Next(0, wallData.Height);
            List<Vector2Int> visitedPositions = new List<Vector2Int>();
            VisitCell(wallData, new Vector2Int(currentX, currentY), visitedPositions);

            while(visitedPositions.Count < wallData.Width * wallData.Height)
            {
                List<Neighbor> unvisitedNeighbors = GetUnvisitedNeighbors(new Vector2Int(currentX, currentY), wallData);
                if(unvisitedNeighbors.Count > 0)
                {
                    Neighbor nextNeighbor = unvisitedNeighbors[rng.Next(0, unvisitedNeighbors.Count)];

                    wallData.ClearWallsBetweenCurrentAndNext(nextNeighbor, new(currentX, currentY), nextNeighbor.Position);

                    VisitCell(wallData, nextNeighbor.Position, visitedPositions);
                    currentX = nextNeighbor.Position.x;
                    currentY = nextNeighbor.Position.y;
                }
                else
                {
                    // choose a random visited cell to move to
                    int index = rng.Next(0, visitedPositions.Count);
                    Vector2Int visitedPos = visitedPositions[index];
                    currentX = visitedPos.x;
                    currentY = visitedPos.y;
                }
            }

            return wallData;
        }

        void VisitCell(MazeWallData maze, Vector2Int pos, List<Vector2Int> visitedPositions)
        {
            maze.SetVisited(pos);
            visitedPositions.Add(pos);
        }
    }
}