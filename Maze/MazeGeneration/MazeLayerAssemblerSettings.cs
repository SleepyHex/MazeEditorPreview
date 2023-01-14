namespace MazeGen
{
    [System.Serializable]
    public class MazeLayerAssemblerSettings
    {
        public bool bShouldUseObjectPool;

        public float tileSize;
        public float floorTileThickness;
        public float wallTileHeight;
        public float wallTileThickness;
        public bool sealedBottomMaze; // prob refactor with other settings options soon i.e allignment type

        public MazeLayerAssemblerSettings(bool ShouldUseObjectPooling, float FloorTileSize, float FloorTileThickness, float WallTileSize, float WallTileThickness, bool SealedBottom)
        {
            bShouldUseObjectPool = ShouldUseObjectPooling;
            tileSize = FloorTileSize;
            floorTileThickness = FloorTileThickness;
            wallTileHeight = WallTileSize;
            wallTileThickness = WallTileThickness;
            sealedBottomMaze = SealedBottom;
        }
    }
}