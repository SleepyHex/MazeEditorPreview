using System.Collections.Generic;
using UnityEngine;

namespace MazeGen
{
    public class MazeFloorMap : MonoBehaviour
    {
        public Dictionary<Vector2Int, MazeCell> MazeCells { get; set; } = new();
    }
}