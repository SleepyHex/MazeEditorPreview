using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MazeGen
{
    public class WallTileFactory
    {
        bool shouldUseObjPool;
        public WallTile LoadSingleCellWallTiles(MazeLayer currentMaze, MazeWallData mazeWallData, int x, int y, float tileSize, float wallTileThickness, float wallTileHeight, Transform parent, float floorTileThickness, GameObject WallTilePrefab, bool bShouldUseObjectPool, bool SealedBottom)
        {
            shouldUseObjPool = bShouldUseObjectPool;

            float heightValue = wallTileHeight / 2f + 0.001f;
            float AntiTextureMushingMultiplier = .9995f;
            heightValue -= floorTileThickness / 2f;

            var cell = mazeWallData.Walls[x, y];
            var position = Vector3.zero;

            bool isRightEdgeOfMaze = (x == currentMaze.Size.x - 1);
            bool isBottomEdgeOfMaze = (y == 0);

            WallParams wallParams = new(WallTilePrefab, position, heightValue, tileSize, wallTileThickness, wallTileHeight, AntiTextureMushingMultiplier, parent, x, y);
            CreateWalls(cell, wallParams, SealedBottom, floorTileThickness, isRightEdgeOfMaze, isBottomEdgeOfMaze);

            return wallParams.parent.AddComponent<WallTile>();
        }

        private void CreateWalls(WallState cell, WallParams wallParams, bool SealedBottom, float floorTileThickness, bool isRightEdgeOfMaze, bool isBottomEdgeOfMaze)
        {
            if(cell.HasFlag(WallState.UP))
                CreateWall(WallState.UP, 0, "UP", wallParams, SealedBottom, floorTileThickness);

            if(cell.HasFlag(WallState.LEFT))
                CreateWall(WallState.LEFT, 90, "LEFT", wallParams, SealedBottom, floorTileThickness);

            if(isRightEdgeOfMaze)//only check for right and bottom wall if it is far right edge or far bottom edge of maze, as to not have cells with left and right walls in the same spot
            {
                if(cell.HasFlag(WallState.RIGHT))
                    CreateWall(WallState.RIGHT, 90, "RIGHT", wallParams, SealedBottom, floorTileThickness);
            }
            if(isBottomEdgeOfMaze)
            {
                if(cell.HasFlag(WallState.DOWN))
                    CreateWall(WallState.DOWN, 0, "DOWN", wallParams, SealedBottom, floorTileThickness);
            }
        }

        void CreateWall(WallState wallState, int eulerAngles, string wallNameSuffix, WallParams wallParams, bool sealedBottom, float floorTileThickness)
        {
            float desiredTileYPos = 0f;
            if(!sealedBottom)
                desiredTileYPos += floorTileThickness;

            desiredTileYPos += wallParams.tileHeight / 2f - floorTileThickness / 2f;

            var wall = GetNewWallTile(wallParams.wallTilePrefab).transform;
            wall.position = GetWallPosition(wallState, wallParams.position, desiredTileYPos, wallParams.tileSize);
            wall.localScale = GetWallScale(wallState, wallParams.tileSize, wallParams.tileThickness, wallParams.tileHeight, wallParams.antiTextureMushMult);
            wall.eulerAngles = new Vector3(0, eulerAngles, 0);
            wall.parent = wallParams.parent;
            wall.name = $"Wall [{wallNameSuffix}]";
            wall.AddComponent<WallTile>();

            Vector3 GetWallPosition(WallState wallState, Vector3 position, float desiredTileYPos, float wallTileScale)
            {
                if(wallState == WallState.UP)
                    return position + new Vector3(0f, desiredTileYPos, wallTileScale / 2f);
                else if(wallState == WallState.DOWN)
                    return position + new Vector3(0f, desiredTileYPos, -wallTileScale / 2f);
                else if(wallState == WallState.RIGHT)
                    return position + new Vector3(wallTileScale / 2f, desiredTileYPos, 0f);
                else
                    return position + new Vector3(-wallTileScale / 2f, desiredTileYPos, 0f);
            }

            Vector3 GetWallScale(WallState wallState, float tileSize, float wallThx, float wallHeight, float antiTextureMushMult)
            {
                if(wallState == WallState.UP || wallState == WallState.DOWN)
                    return new Vector3(tileSize + wallThx, wallHeight, wallThx) * antiTextureMushMult;
                else
                    return new Vector3(tileSize + wallThx, wallHeight - (1 - antiTextureMushMult), wallThx) * antiTextureMushMult;
            }

            GameObject GetNewWallTile(GameObject wallTilePrefab) => shouldUseObjPool ? ObjectPoolManager.Instance.GetPooledObject(typeof(WallTile)) : GameObject.Instantiate(wallTilePrefab);
        }
    }
}