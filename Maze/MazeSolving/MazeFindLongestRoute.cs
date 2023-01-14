using System.Collections.Generic;
using System.Linq;
using MazeGen;
using UnityEngine;

public static class MazeFindLongestRoute
{
    public static void CalcFurthestPossibleAndAssignToMaze(MazeLayer maze, MazeWallData wallData)
    {
        var FurthestNodeInfo = FindFurthestPossibleNode(wallData, maze.Seed);
        maze.Goal = FurthestNodeInfo.Value.pos;
        maze.Spawn = FurthestNodeInfo.Key;
    }

    public static MazeCell GetFurthestFloorTileFromSpawn()
    {
        var MazeDataObj = GetMazeData.MazeFloorMap;
        if(MazeDataObj == null)
            GetMazeData.UpdateMazeFloorMap();

        if(MazeDataObj != null)
        {
            List<MazeCell> values = MazeDataObj.MazeCells.Values.ToList();

            int furthest = 0;
            Node furthestNode = values[0].node;
            foreach(var tile in values.Where(tile => tile.node.distanceFromStart > furthest))
            {
                furthest = tile.node.distanceFromStart;
                furthestNode = tile.node;
            }

            if(furthestNode != null)
                return furthestNode.MazeCellRef;
        }
        return null;
    }

    static KeyValuePair<Vector2Int, Node> FindFurthestPossibleNode(MazeWallData wallData, int seed)
    {
        int furthestDist = 0;
        Vector2Int spawnLinkedToFurthest = Vector2Int.zero;

        MazeCell furthestTile = GetFurthestFloorTileFromSpawn();
        for(int x = 0; x < wallData.Width; x++)
        {
            for(int y = 0; y < wallData.Height; y++)
            {
                Vector2Int currentBoardPos = new Vector2Int(x,y);
                MazeSolver.SolveMaze(wallData, currentBoardPos);
                var tempFurthestTile = GetFurthestFloorTileFromSpawn();

                if(tempFurthestTile.node.distanceFromStart > furthestDist)
                {
                    furthestDist = tempFurthestTile.node.distanceFromStart;
                    furthestTile = tempFurthestTile;
                    spawnLinkedToFurthest = currentBoardPos;
                }
            }
        }

        Debug.Log("Actual Furthest determined for seed " + seed + ": " + furthestTile.node.pos + ", " + spawnLinkedToFurthest + " " + furthestDist);
        return new KeyValuePair<Vector2Int, Node>(spawnLinkedToFurthest, furthestTile.node);
    }
}
