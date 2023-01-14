using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGen
{
    [System.Serializable]
    public class MazeLayerStackAssemblerSettings
    {
        public float gapBetweenFloors;
        public bool gridSafeAllignment;

        public MazeLayerStackAssemblerSettings(float distanceBetweenFloors)
        {
            this.gapBetweenFloors = distanceBetweenFloors;
        }
    }
}