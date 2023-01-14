using System.Linq;
using UnityEngine;
using MazeGen;

public static class GetMazeData
{
    public static MazeFloorMap MazeFloorMap { get; set; }
    public static void UpdateMazeFloorMap() => MazeFloorMap = GameObject.FindObjectOfType<MazeFloorMap>();

    public static MazeCell GetCell(Vector2Int boardPos)
    {
        if(MazeFloorMap == null)
            UpdateMazeFloorMap();

        if(MazeFloorMap != null)
            if(MazeFloorMap.MazeCells.ContainsKey(boardPos))
                return MazeFloorMap.MazeCells[boardPos];

        return null;
    }

    public static int GetTileCount(MazeLayerStack layerStack)
    {
        if(layerStack.layers != null)
        {
            var cellCt = from layer in layerStack.layers
                         where layer != null
                         select layer.Size.x * layer.Size.y;

            return cellCt.Sum();
        }
        return -1;
    }
}
