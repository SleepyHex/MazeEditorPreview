using MazeEditor;
using UnityEditor;
using UnityEngine;
public static class MazeDebugger
{
#if UNITY_EDITOR
    [MenuItem("Maze Debugger/CalculateFurthestFromSpawn")]
    static void CalculateFurthestFromSpawn()
    {
        var furthestNode = MazeFindLongestRoute.GetFurthestFloorTileFromSpawn();
        Debug.Log("Spawn: " + MazeLayerEditor.currentMaze.Spawn + furthestNode.node.distanceFromStart);
        Debug.Log("Furthest Goal: " + furthestNode.node.pos);
    }
#endif
}