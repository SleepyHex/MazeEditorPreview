using UnityEngine;

namespace MazeGen
{
    public class WallParams
    {
        public GameObject wallTilePrefab { get; }
        public Vector3 position { get; }
        public float heightValue { get; }
        public float tileSize { get; }
        public float tileThickness { get; }
        public float tileHeight { get; }
        public float antiTextureMushMult { get; }
        public Transform parent { get; }
        public int x { get; }
        public int y { get; }

        public WallParams(GameObject wallTilePrefab, Vector3 position, float heightValue, float tileSize, float wallTileThickness, float wallTileHeight, float antiTextureMushMult, Transform parent, int x, int y)
        {
            this.wallTilePrefab = wallTilePrefab;
            this.position = position;
            this.heightValue = heightValue;
            this.tileSize = tileSize;
            this.tileThickness = wallTileThickness;
            this.tileHeight = wallTileHeight;
            this.antiTextureMushMult = antiTextureMushMult;
            this.parent = parent;
            this.x = x;
            this.y = y;
        }
    }
}