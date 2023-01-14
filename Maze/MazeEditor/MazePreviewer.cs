using MazeGen;
using UnityEditor;
using UnityEngine;


namespace MazeEditor
{
#if UNITY_EDITOR
    [ExecuteAlways]
    public class MazePreviewer : MonoBehaviour
    {
        public static readonly GenericEvent<MazeWallData, float> DoPreviewDisplays = new();
        public static readonly GenericEvent<MazeLayer> selectedMazeChanged = new();

        [SerializeField] GameObject textAboveTilePrefab;
        [SerializeField] bool showGoalAndSpawn;
        [SerializeField] bool showMazeSolution;
        [SerializeField] bool showDistances;
        [SerializeField] bool assignFurthestGoalFromSpawn;
        [SerializeField] bool assignFurthestPossibleSpawnAndGoal;
        [SerializeField] bool showDeadEnds;
        [SerializeField] bool showJunctions;
        [SerializeField] Color SpawnColor;
        [SerializeField] Color GoalColor;
        [SerializeField] Color SpawnNodeIdColor;
        [SerializeField] Color GoalNodeIdColor;

        MazeLayer currentMaze { get; set; }
        bool timeForUpdate = false;

        void OnEnable()
        {
            MazeLayerAssembler.MazeSettingsChanged.AddListener(MazeChanged);
            DoPreviewDisplays.AddListener(DoMazeRenderingExtras);
        }

        void OnDisable()
        {
            MazeLayerAssembler.MazeSettingsChanged.RemoveListener(MazeChanged);
            DoPreviewDisplays.RemoveListener(DoMazeRenderingExtras);
        }

        void Update() => RefreshMaze();
        void OnValidate() => MazeChanged();
        void MazeChanged() => timeForUpdate = true;

        void RefreshMaze()
        {
            currentMaze = MazeLayerEditor.currentMaze;

            if(currentMaze != null && !EditorApplication.isPaused)
            {
                if(currentMaze.NeedsUpdating || timeForUpdate)
                {
                    selectedMazeChanged.Invoke(currentMaze);

                    //MazeAssembler.BuildMaze.Invoke(currentMaze);  /////used to refresh to maze build here but now move this to mazelayerstack previewer?
                    currentMaze.FinishUpdateState();
                    timeForUpdate = false;
                }
            }
        }

        void DoMazeRenderingExtras(MazeWallData walls, float tileSize)
        {
            if(!currentMaze)
                RefreshMaze();
            if(!currentMaze)
                return;


            MazeSolutionDisplay.DisplayMazeSolution(currentMaze, walls, tileSize, showMazeSolution, showGoalAndSpawn, GoalColor, SpawnColor);

            if(assignFurthestGoalFromSpawn)
            {
                var newGoal = MazeFindLongestRoute.GetFurthestFloorTileFromSpawn();
                currentMaze.Goal = newGoal.node.pos;
            }

            if(assignFurthestPossibleSpawnAndGoal)
                MazeFindLongestRoute.CalcFurthestPossibleAndAssignToMaze(currentMaze, walls); //overwrites maze goal and spawn


            if(showDistances)
                MazeSolver.DisplayNodeDistances(textAboveTilePrefab, GoalNodeIdColor, SpawnNodeIdColor, MazeFindLongestRoute.GetFurthestFloorTileFromSpawn().node);


            if(showJunctions)
                MazeSolver.DisplayJunctions(textAboveTilePrefab);


            if(showDeadEnds)
                MazeSolver.DisplayDeadEnds(textAboveTilePrefab);
        }
    }
#endif
}