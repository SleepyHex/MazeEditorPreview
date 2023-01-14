using UnityEngine;
using static MazeAlgos.MazeAlgorithmChoices;

namespace MazeGen
{
    [CreateAssetMenu(menuName = "Maze", fileName = "Maze")]
    [System.Serializable]
    public class MazeLayer : ScriptableObject
    {
        public Vector2Int MaxMazeSize { get; } = new(250, 250);
        public AlgorithmChoice GenerationAlgorithm;
        public int Seed;
        public Vector2Int Size = Vector2Int.one;
        public Vector2Int Spawn;
        public Vector2Int Goal;

        public bool NeedsUpdating { get; set; } = false; //this "needs updating" crap is a workaround for the fact we can't send events in OnValidate of a scriptableobject
        public void FinishUpdateState() => NeedsUpdating = false;
        public void SetNeedUpdate() => NeedsUpdating = true;
        void OnValidate() => NeedsUpdating = true;
    }
}