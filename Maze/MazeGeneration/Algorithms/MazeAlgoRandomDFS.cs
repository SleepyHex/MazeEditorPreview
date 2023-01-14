using System.Collections.Generic;
using MazeGen;
using UnityEngine;
using static MazeGen.MazeGenHelper;
using Random = System.Random;

namespace MazeAlgos
{
    public class MazeAlgoRandomDFS : IMazeAlgorithm
    {
        public override MazeWallData ExecuteAlgorithm(MazeWallData wallData, int Seed)
        {
            int width = wallData.Width;
            int height = wallData.Height;

            if(width <= 0 || height <= 0)
                return null;

            Random rng = new System.Random(Seed);
            Stack<Vector2Int> positionStack = new Stack<Vector2Int>();
            Vector2Int position = new(rng.Next(0, (int)width), rng.Next(0, (int)height));

            wallData.SetVisited(position);
            positionStack.Push(position);

            while(positionStack.Count > 0)
            {
                Vector2Int currentPos = positionStack.Pop();
                List<Neighbor> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos, wallData);

                if(unvisitedNeighbors.Count > 0)
                {
                    positionStack.Push(currentPos);

                    int randIndex = rng.Next(0, unvisitedNeighbors.Count);
                    Neighbor randomNeighbour = unvisitedNeighbors[randIndex];

                    Vector2Int nPosition = randomNeighbour.Position;
                    wallData.ClearWallsBetweenCurrentAndNext(randomNeighbour, currentPos, nPosition);

                    wallData.SetVisited(nPosition);

                    positionStack.Push(nPosition);
                }
            }
            return wallData;
        }
    }
}