using UnityEngine;

namespace MazeGen
{
    public class MazeCleanup : MonoBehaviour
    {
        void CleanupMaze(bool useObjectPooling, MazeFloorMap mazeMap)
        {
            foreach(var cell in mazeMap.MazeCells)
            {
                Destroy(cell.Value.FloorTile.gameObject);
                Destroy(cell.Value.WallTile.gameObject);
            }

            //foreach(var v in mazeMap.MazeCells)
            //    if(useObjectPooling)
            //        ObjectPoolManager.Instance.ReturnObjectToPool(v.Value.gameObject, typeof(MazeCell));
            //    else
            //        Destroy(v.Value.gameObject);

            //foreach(var v in mazeMap.wall)
            //    if(useObjectPooling)
            //        ObjectPoolManager.Instance.ReturnObjectToPool(v.Value.gameObject, typeof(MazeCell));
            //    else
            //        Destroy(v.Value.gameObject);
        }
    }
}
