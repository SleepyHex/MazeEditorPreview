using Assets.Scripts.Extensions;
using MazeEditor;
using UnityEngine;

namespace MazeGen
{
    public class MazeLayerAssembler : MonoBehaviour
    {
        [SerializeField] GameObject floorTilePrefab;
        [SerializeField] GameObject wallTilePrefab;
        [SerializeField] MazeLayerAssemblerSettings settings = new(false, 1f, .2f, 1f, .2f, false);


        public static readonly GenericEvent<MazeLayer> AssembleLayer = new();
        public static readonly GenericEvent<GameObject> MazeLayerAssemblyCompleted = new();
        public static readonly GenericEvent MazeSettingsChanged = new();

        public MazeLayer CurrentMaze { get; private set; }
        public MazeWallData MazeWallStates { get; private set; }
        FloorTileFactory FloorTileFactory { get; } = new();
        WallTileFactory WallTileFactory { get; } = new();
        string mazeName => @"Maze '" + (CurrentMaze?.name ?? "NULL") + @"' (" + (CurrentMaze?.Size.x ?? 0) + "," + (CurrentMaze?.Size.y ?? 0) + ")";

        bool spawningNow = false;
        GameObject layerParent;
        GameObject floorParent;
        MazeFloorMap mazeFloorMap;


        void OnEnable() => AssembleLayer.AddListener(SpawnMazeLayerIntoGameWorld);
        void OnDisable() => AssembleLayer.RemoveListener(SpawnMazeLayerIntoGameWorld);
        void OnValidate() => MazeSettingsChanged.Invoke();

        void SpawnMazeLayerIntoGameWorld(MazeLayer maze)
        {
            if(spawningNow) return;
            spawningNow = true;

            CurrentMaze = maze;
            InitializeMazeLayer(CurrentMaze);
            GenerateMazeLayerWalls(CurrentMaze);
            PlaceFloorAndWallTiles(CurrentMaze);
            SolveMazeLayer(CurrentMaze);
            PreviewMazeLayer(CurrentMaze);


            MazeLayerAssemblyCompleted.Invoke(layerParent.gameObject);
            spawningNow = false;
        }

        void InitializeMazeLayer(MazeLayer maze)
        {
            //transform.DestroyAllChildren(true); //used to be needed for single layer assembly via mazepreviewer. prob just move to stack assembler

            layerParent = GameObjectExtras.AssignNewGameObjAndGiveParent(mazeName, transform);
            floorParent = GameObjectExtras.AssignNewGameObjAndGiveParent("Tiles", layerParent.transform);

            mazeFloorMap = layerParent.AddComponent<MazeFloorMap>();
        }

        void GenerateMazeLayerWalls(MazeLayer maze) => MazeWallStates = MazeWallGenerator.MazeGenerator.Generate(CurrentMaze.GenerationAlgorithm, CurrentMaze.Size, CurrentMaze.Seed);

        void SolveMazeLayer(MazeLayer maze) => MazeSolver.SolveMaze(MazeWallStates, CurrentMaze.Spawn);

        void PreviewMazeLayer(MazeLayer maze) => MazePreviewer.DoPreviewDisplays.Invoke(MazeWallStates, settings.tileSize);

        void PlaceFloorAndWallTiles(MazeLayer maze)
        {
            for(int x = 0; x < CurrentMaze.Size.x; x++)
            {
                for(int y = 0; y < CurrentMaze.Size.y; y++)
                {
                    Vector2Int boardPos = new(x,y);
                    GameObject newCellObject = new("Cell [" + x + ", " + y + "]");
                    newCellObject.transform.SetParent(floorParent.transform);


                    FloorTile floor = PlaceFloorTileAtCell(CurrentMaze, MazeWallStates, x, y, newCellObject.transform, settings.bShouldUseObjectPool, mazeFloorMap);
                    floor.transform.parent = newCellObject.transform;

                    WallTile walls = PlaceWallTilesAtCell(CurrentMaze, MazeWallStates, x, y, newCellObject.transform, settings.bShouldUseObjectPool);

                    MazeCell cellComponent = newCellObject.AddComponent<MazeCell>();
                    cellComponent.Init(boardPos, MazeWallStates, floor, walls, settings);

                    floor.cellParent = cellComponent;
                    walls.cellParent = cellComponent;

                    if(!mazeFloorMap.MazeCells.ContainsKey(boardPos))
                        mazeFloorMap.MazeCells[boardPos] = cellComponent;
                }
            }
        }

        public FloorTile PlaceFloorTileAtCell(MazeLayer maze, MazeWallData mazeWallStates, int x, int y, Transform parent, bool useObjectPooling, MazeFloorMap mazeDataObject)
            => FloorTileFactory.LoadSingleCellFloorTile(maze, mazeWallStates, x, y, settings.tileSize, settings.floorTileThickness, transform, floorTilePrefab, useObjectPooling, mazeDataObject, Vector3.zero);

        public WallTile PlaceWallTilesAtCell(MazeLayer maze, MazeWallData mazeWallStates, int x, int y, Transform parent, bool useObjectPooling)
            => WallTileFactory.LoadSingleCellWallTiles(maze, mazeWallStates, x, y, settings.tileSize, settings.wallTileThickness, settings.wallTileHeight, parent, settings.floorTileThickness, wallTilePrefab, useObjectPooling, settings.sealedBottomMaze);
    }
}