using System.Collections.Generic;
using System.Linq;
using MazeGen;
using TMPro;
using UnityEngine;

public static class MazeSolutionDisplay
{
    static BreadcrumbHelper bch = new();
    public static void DisplayMazeSolution(MazeLayer maze, MazeWallData wallData, float tileSize, bool showMazeSolutionFull, bool showSpawnAndGoal, Color goalColor, Color spawnColor)
    {
        if(showMazeSolutionFull)
            PlaceSolutionBreadcrumbs(maze, wallData, tileSize, goalColor, spawnColor);
        if(showSpawnAndGoal)
            PlaceGoalAndSpawnBreadcrumbs(maze, tileSize, goalColor, spawnColor);
    }

    static void PlaceGoalAndSpawnBreadcrumbs(MazeLayer maze, float tileSize, Color goalColor, Color spawnColor)
    {
        var goalTile = GetMazeData.GetCell(maze.Goal);
        bch.AddBreadCrumb(goalTile, tileSize / 2f, goalTile.transform, goalColor);

        var spawnTile = GetMazeData.GetCell(maze.Spawn);
        bch.AddBreadCrumb(spawnTile, tileSize / 2f, spawnTile.transform, spawnColor);
    }

    static void PlaceSolutionBreadcrumbs(MazeLayer maze, MazeWallData wallData, float tileSize, Color goalColor, Color spawnColor)
    {
        var currentTile = GetMazeData.GetCell(maze.Goal);
        if(!currentTile) return;

        int startingDistance = currentTile.node.distanceFromStart;
        float stepsTakenBack = 0f;

        for(int i = currentTile.node.distanceFromStart; i > 0; i--)
        {
            if(currentTile.node.pos != maze.Goal && currentTile.node.pos != maze.Spawn)
                bch.AddBreadCrumb(currentTile, tileSize, currentTile.transform, Color.Lerp(goalColor, spawnColor, stepsTakenBack / (float)startingDistance));

            List<Node> neighbors = currentTile.node.GetTransversableNeighbors();
            if(neighbors.Count > 0)
                foreach(var neighbor in neighbors.Where(neighbor => neighbor.distanceFromStart == i - 1))
                {
                    currentTile = neighbor.MazeCellRef;
                    break;
                }

            stepsTakenBack++;
        }
    }

    public static void ApplyTextToFloorTile(GameObject floorTextPrefab, Color GoalColor, Color SpawnColor, float startingDistance, FloorTile tile)
    {
        var newText = GameObject.Instantiate(floorTextPrefab);
        newText.transform.position = tile.transform.position + new Vector3(0f, tile.transform.localScale.y + 0.02f, 0f);
        ApplyColorForBreadcrumbs(GoalColor, SpawnColor, startingDistance, tile.cellParent, newText);
        newText.transform.parent = tile.transform;
    }

    static void ApplyColorForBreadcrumbs(Color GoalColor, Color SpawnColor, float startingDistance, MazeCell tile, GameObject newText)
    {
        newText.GetComponentInChildren<TextMeshProUGUI>().text = tile.node.distanceFromStart.ToString();
        newText.GetComponentInChildren<TextMeshProUGUI>().color = Color.Lerp(SpawnColor, GoalColor, (float)tile.node.distanceFromStart / startingDistance);
    }
}
