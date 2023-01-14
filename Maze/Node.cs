using System.Collections.Generic;
using System.Linq;
using MazeGen;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int pos { get; set; }
    public MazeCell MazeCellRef { get; set; }
    public int distanceFromStart { get; set; } = -1;
    public int junctionCount { get; set; } = -1;
    public bool isDeadEnd => junctionCount <= 1;


    readonly Dictionary<MoveDirection, Node> neighbors;
    MazeWallData WallData;

    public Node(MazeCell MazeCellParent, Vector2Int Pos, MazeWallData wallData)
    {
        MazeCellRef = MazeCellParent;
        pos = Pos;
        neighbors = new Dictionary<MoveDirection, Node>();
        WallData = wallData;
    }

    public void AddNeighbor(MoveDirection direction, Node neighbor) => neighbors[direction] = neighbor;

    public List<Node> GetTransversableNeighbors()
    {
        UpdateNeighbors();
        return neighbors.Values.ToList();
    }

    public int GetTransversableNeighborCount()
    {
        UpdateNeighbors();
        return neighbors.Count;
    }

    public Node TryGetNeighbor(MoveDirection desiredDir)
    {
        neighbors.TryGetValue(desiredDir, out Node neighbor);
        return neighbor;
    }

    void UpdateNeighbors()
    {
        var currentPos = pos;

        if(currentPos.y < WallData.Height - 1) //up
            if(!WallData.Walls[currentPos.x, currentPos.y].HasFlag(WallState.UP))
            {
                var neighbor = GetMazeData.GetCell(new(currentPos.x, currentPos.y + 1)).node;
                neighbors[MoveDirection.UP] = neighbor;
            }

        if(currentPos.x > 0) //left
            if(!WallData.Walls[currentPos.x, currentPos.y].HasFlag(WallState.LEFT))
            {
                var neighbor = GetMazeData.GetCell(new(currentPos.x - 1, currentPos.y)).node;
                neighbors[MoveDirection.LEFT] = neighbor;
            }

        if(currentPos.x < WallData.Width - 1) //right
            if(!WallData.Walls[currentPos.x, currentPos.y].HasFlag(WallState.RIGHT))
            {
                var neighbor = GetMazeData.GetCell(new(currentPos.x + 1, currentPos.y)).node;
                neighbors[MoveDirection.RIGHT] = neighbor;
            }

        if(currentPos.y > 0) //down
            if(!WallData.Walls[currentPos.x, currentPos.y].HasFlag(WallState.DOWN))
            {
                var neighbor = GetMazeData.GetCell(new(currentPos.x, currentPos.y - 1)).node;
                neighbors[MoveDirection.DOWN] = neighbor;
            }

        junctionCount = neighbors.Count;
    }
}