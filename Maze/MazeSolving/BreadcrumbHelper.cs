using UnityEngine;

namespace MazeGen
{
    public class BreadcrumbHelper
    {
        BreadCrumb bc = new();
        public void AddBreadCrumb(MazeCell mazeCell, float floorTileSize, Transform parent, Color color) => bc.PlaceBreadcrumb(mazeCell.FloorTile.transform.position + new Vector3(0f, (mazeCell.FloorTile.transform.localScale.y / 2), 0f), color, floorTileSize).SetParent(parent);
    }
}