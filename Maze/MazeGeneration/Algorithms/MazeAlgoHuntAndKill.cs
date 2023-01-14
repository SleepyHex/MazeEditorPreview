using System.Collections.Generic;
using MazeGen;
using UnityEngine;
using static MazeGen.MazeGenHelper;
using Random = System.Random;


namespace MazeAlgos
{
    public class MazeAlgoHuntAndKill : IMazeAlgorithm
    {
        public override MazeWallData ExecuteAlgorithm(MazeWallData wallData, int Seed)
        {
            int width = wallData.Width;
            int height = wallData.Height;
            if(width == 0 || height == 0)
                return null;

            Random rng = new System.Random(Seed);
            Vector2Int currentPos = new Vector2Int(rng.Next(0, width), rng.Next(0, height));
            wallData.SetVisited(currentPos);

            while(DetermineIfGridContainsUnvisited(wallData))
            {
                bool foundUnvisited = false;
                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        currentPos.x = i;
                        currentPos.y = j;
                        if(wallData.IsVisited(currentPos.x, currentPos.y))
                            continue;

                        List<Neighbor> visitedNeighbours = GetVisitedNeighbors(currentPos, wallData);

                        if(visitedNeighbours.Count == 0)
                            continue;

                        Neighbor randomVisitedNeighbor = visitedNeighbours[rng.Next(0, visitedNeighbours.Count)];
                        wallData.ClearWallsBetweenCurrentAndNext(randomVisitedNeighbor, currentPos, randomVisitedNeighbor.Position);
                        wallData.SetVisited(currentPos);
                        foundUnvisited = true;
                    }
                }

                if(!foundUnvisited)
                {
                    for(int i = 0; i < width; i++)
                    {
                        for(int j = 0; j < height; j++)
                        {
                            currentPos.x = i;
                            currentPos.y = j;
                            if(wallData.IsVisited(currentPos))
                                continue;

                            List<Neighbor> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos, wallData);
                            if(unvisitedNeighbors.Count > 0)
                            {
                                Neighbor randomUnvisitedNeighbour = unvisitedNeighbors[rng.Next(0, unvisitedNeighbors.Count)];
                                wallData.ClearWallsBetweenCurrentAndNext(randomUnvisitedNeighbour, currentPos, randomUnvisitedNeighbour.Position);
                                wallData.SetVisited(randomUnvisitedNeighbour.Position.x, randomUnvisitedNeighbour.Position.y);
                                foundUnvisited = true;
                                break;
                            }
                        }
                        if(foundUnvisited)
                            break;
                    }
                }
            }
            return wallData;
        }
    }
}