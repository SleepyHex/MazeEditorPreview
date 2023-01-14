using System.Collections.Generic;
using MazeGen;
using UnityEngine;

namespace MazeGen
{
    [CreateAssetMenu(menuName = "Maze Layer Stack")]
    public class MazeLayerStack : ScriptableObject
    {
        public List<MazeLayer> layers;
    }
}