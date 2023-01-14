using System;
using UnityEngine;

namespace MazeGen
{
    [Flags]
    public enum WallState
    {
        LEFT    = 1,   //0001
        RIGHT   = 2,   //0010
        UP      = 4,   //0100
        DOWN    = 8,   //1000
        VISITED = 128  //1000 0000
    }

    public struct Neighbor
    {
        public Vector2Int Position;
        public WallState SharedWall;

        public Neighbor(Vector2Int pos, WallState sharedWall)
        {
            Position = pos;
            SharedWall = sharedWall;
        }
    }
}