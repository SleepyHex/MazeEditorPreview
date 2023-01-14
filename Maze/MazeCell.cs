using MazeGen;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Node node { get; private set; }
    public FloorTile FloorTile { get; private set; }
    public WallTile WallTile { get; private set; }

    public void Init(Vector2Int pos, MazeWallData wallData, FloorTile floorTile, WallTile wallTile, MazeLayerAssemblerSettings settings)
    {
        FloorTile = floorTile;
        WallTile = wallTile;

        float mazeFloorWidth = wallData.Width * settings.tileSize;
        float mazeFloorLength = wallData.Height * settings.tileSize;

        float xPos = -mazeFloorWidth / 2 + (pos.x * settings.tileSize) + settings.tileSize / 2;
        float yPos = settings.floorTileThickness / 2;
        float zPos = -mazeFloorLength / 2 + (pos.y * settings.tileSize) + settings.tileSize / 2;
        transform.position = new(xPos, yPos, zPos);

        node = new(this, pos, wallData);
    }
}