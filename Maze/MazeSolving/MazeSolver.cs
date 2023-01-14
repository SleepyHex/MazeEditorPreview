using System.Collections.Generic;
using System.Linq;
using MazeGen;
using TMPro;
using UnityEngine;

public static class MazeSolver
{
    static int currentDistFromStart;
    static HashSet<MazeCell> mazeCells;
    public static void SolveMaze(MazeWallData mazeWallData, Vector2Int starPos) => CalculateAllNodeDistances(mazeWallData, starPos);

    static void CalculateAllNodeDistances(MazeWallData mazeWallData, Vector2Int starPos)
    {
        mazeCells = new();
        currentDistFromStart = 0;
        MazeCell startingTile = GetMazeData.GetCell(starPos);

        if(!startingTile)
            return;

        startingTile.node.distanceFromStart = 0;
        mazeCells.Add(startingTile);

        while(currentDistFromStart < mazeWallData.Width * mazeWallData.Height)
        {
            var currentCells = new HashSet<MazeCell>(mazeCells);
            foreach(var cell in currentCells)
                CalculateNodeNeighbors(cell);

            currentDistFromStart++;
        }
    }

    public static void DisplayNodeDistances(GameObject floorTextPrefab, Color GoalColor, Color SpawnColor, Node furthestNode)
    {
        foreach(var mazeCell in mazeCells)
            MazeSolutionDisplay.ApplyTextToFloorTile(floorTextPrefab, GoalColor, SpawnColor, furthestNode.distanceFromStart, mazeCell.FloorTile);
    }

    public static void DisplayJunctions(GameObject floorTextPrefab)
    {
        foreach(var cell in mazeCells)
        {
            if(cell.node.junctionCount <= 2) continue;
            SetNewText(floorTextPrefab, cell, cell.node.junctionCount.ToString());
        }
    }

    public static void DisplayDeadEnds(GameObject floorTextPrefab)
    {
        foreach(var cell in mazeCells)
        {
            if(!cell.node.isDeadEnd) continue;
            SetNewText(floorTextPrefab, cell, "Dead\nEnd");
        }
    }

    static void CalculateNodeNeighbors(MazeCell currentCell)
    {
        if(currentCell.node.distanceFromStart != currentDistFromStart)
            return;

        var neighbors = currentCell.node.GetTransversableNeighbors();
        foreach(var neighbor in neighbors.Where(subTile => !mazeCells.Contains(subTile.MazeCellRef)))
        {
            mazeCells.Add(neighbor.MazeCellRef);
            AssignNodeDistanceValues(neighbor, currentDistFromStart + 1);
        }
    }

    static void AssignNodeDistanceValues(Node node, int distFromStart)
    {
        node.distanceFromStart = distFromStart;
        AssignNodeDistanceValuesInDirection(node, distFromStart, MoveDirection.UP);
        AssignNodeDistanceValuesInDirection(node, distFromStart, MoveDirection.DOWN);
        AssignNodeDistanceValuesInDirection(node, distFromStart, MoveDirection.LEFT);
        AssignNodeDistanceValuesInDirection(node, distFromStart, MoveDirection.RIGHT);
    }

    static void AssignNodeDistanceValuesInDirection(Node node, int distFromStart, MoveDirection direction)
    {
        Node neighbor = node.TryGetNeighbor(direction) ?? null;
        if(neighbor != null && neighbor.distanceFromStart == -1)
            neighbor.distanceFromStart = distFromStart + 1;
    }

    static void SetNewText(GameObject floorTextPrefab, MazeCell mazeCell, string text)
    {
        var newText = GameObject.Instantiate(floorTextPrefab);
        newText.transform.position = mazeCell.FloorTile.transform.position + new Vector3(0f, mazeCell.FloorTile.transform.localScale.y + 0.02f, 0f);
        newText.transform.parent = mazeCell.FloorTile.transform;

        var textComponent = newText.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.color = Color.red;
    }
}
