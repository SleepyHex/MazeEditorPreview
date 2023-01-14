using System.Collections.Generic;
using System.Linq;
using MazeGen;
using UnityEngine;
using static MazeGen.MazeGenHelper;
using Random = System.Random;

namespace MazeAlgos
{
    public class MazeAlgoWilsons : IMazeAlgorithm
    {
        public override MazeWallData ExecuteAlgorithm(MazeWallData wallData, int seed)
        {
            int width = wallData.Width;
            int height = wallData.Height;
            if(width <= 0 || height <= 0)
                return null;

            Random rng = new System.Random(seed);
            Vector2Int currentPos = new Vector2Int(rng.Next(0, (int)width), rng.Next(0, (int)height));
            List<Vector2Int> visitedPositions = new();


            VisitCell(wallData, currentPos, visitedPositions);

            while(visitedPositions.Count < width * height)
            {
                List<Neighbor> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos, wallData);
                if(unvisitedNeighbors.Count > 0)
                {
                    Neighbor nextNeighb = unvisitedNeighbors[rng.Next(0, unvisitedNeighbors.Count)];

                    VisitCell(wallData, nextNeighb.Position, visitedPositions);

                    wallData.ClearWallsBetweenCurrentAndNext(nextNeighb, currentPos, nextNeighb.Position);
                    currentPos = nextNeighb.Position;
                }
                else
                {
                    currentPos = GetRandomVisitedCell(visitedPositions, rng);
                }

                TakeRandomWalk(wallData, currentPos, width, height, visitedPositions, rng);
            }
            return wallData;
        }

        void VisitCell(MazeWallData maze, Vector2Int currentPos, List<Vector2Int> visitedPositions)
        {
            maze.SetVisited(currentPos.x, currentPos.y);
            visitedPositions.Add(currentPos);
        }

        Vector2Int GetRandomVisitedCell(List<Vector2Int> visitedCells, Random rng)
        {
            int index = rng.Next(0, visitedCells.Count);
            return visitedCells.ElementAt(index);
        }

        void TakeRandomWalk(MazeWallData wallData, Vector2Int startPos, int width, int height, List<Vector2Int> visitedPositions, Random rng)
        {
            Vector2Int currentPos = startPos;
            while(true)
            {
                List<Neighbor> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos, wallData);
                if(unvisitedNeighbors.Count > 0)
                {
                    Neighbor nextNeighb = unvisitedNeighbors[rng.Next(0, unvisitedNeighbors.Count)];
                    VisitCell(wallData, nextNeighb.Position, visitedPositions);
                    wallData.ClearWallsBetweenCurrentAndNext(nextNeighb, currentPos, nextNeighb.Position);
                    currentPos = nextNeighb.Position;
                }
                else
                    break;
            }
        }
    }
}
