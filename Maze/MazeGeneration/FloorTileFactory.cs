using UnityEngine;

namespace MazeGen
{
    public class FloorTileFactory
    {
        bool shouldUseObjPool;
        public FloorTile LoadSingleCellFloorTile(MazeLayer currentMaze, MazeWallData wallData, int x, int y, float tileSize, float tileThickness, Transform parent, GameObject FloorTilePrefab, bool bShouldUseObjectPool, MazeFloorMap mazeDataObj, Vector3 pos)
        {
            shouldUseObjPool = bShouldUseObjectPool;

            var floorTile = GetNewFloorTile(FloorTilePrefab);
            floorTile.name = "Floor Tile";
            floorTile.transform.localScale = new Vector3(tileSize, tileThickness, tileSize);
            floorTile.transform.position = Vector3.zero;
            floorTile.transform.SetParent(parent);

            return floorTile.AddComponent<FloorTile>();
        }

        GameObject GetNewFloorTile(GameObject floorTilePrefab) => shouldUseObjPool ? ObjectPoolManager.Instance.GetPooledObject(typeof(MazeCell)) : GameObject.Instantiate(floorTilePrefab);
    }
}