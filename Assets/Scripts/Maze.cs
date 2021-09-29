using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public HexGrid CurrentMaze;

    public void Start()
    {
        CurrentMaze = GenerateMaze(10);
    }

    public HexGrid GenerateMaze(int radius)
    {
        // set up structures
        HexGrid maze = new HexGrid(radius);
        Stack<Vector3Int> stack = new Stack<Vector3Int>();

        // select starting point from bottom left or bottom
        int startX, startY, startZ;
        if (Random.Range(0, 2) == 0)
        {
            startY = radius; // start on bottom left side
            startX = -Random.Range(0, radius + 1);
            startZ = -(startY + startX);
        }
        else
        {
            startZ = -radius; // start on bottom side
            startY = Random.Range(0, radius + 1);
            startX = -(startZ + startY);
        }
        stack.Push(new Vector3Int(startX, startY, startZ));

        // carving out the maze
        while (stack.Count != 0)
        {
            Vector3Int point = stack.Pop();

            // check if the maze can be expanded to this point
            if (CanExpandMaze(maze, point))
            {
                // put cell on path
                HexCell selected = maze.GetHexAtPosition(point.x, point.y, point.z);
                selected.SetOpen();
                
                // get adjacent points
                Vector3Int[] neighoringPoints = new Vector3Int[]
                {
                    new Vector3Int(point.x, point.y - 1, point.z + 1),
                    new Vector3Int(point.x + 1, point.y - 1, point.z),
                    new Vector3Int(point.x + 1, point.y, point.z - 1),
                    new Vector3Int(point.x, point.y + 1, point.z - 1),
                    new Vector3Int(point.x - 1, point.y + 1, point.z),
                    new Vector3Int(point.x - 1, point.y, point.z + 1)
                };
                Shuffle(neighoringPoints);
                for (int i = 0; i < neighoringPoints.Length; i++)
                {
                    stack.Push(neighoringPoints[i]);
                }
            }
        }
        
        return maze;
    }

    public bool CanExpandMaze(HexGrid maze, Vector3Int point)
    {
        // check if the tile is in the bounds of the maze
        if (InBounds(maze, point))
        {
            return !maze.GetHexAtPosition(point.x, point.y, point.z).isOpen;
        }
        else
        {
            return false;
        }
    }

    public bool InBounds(HexGrid maze, Vector3Int point)
    {
        return Mathf.Abs(point.x) < maze.OuterRadius && Mathf.Abs(point.y) < maze.OuterRadius && Mathf.Abs(point.z) < maze.OuterRadius;
    }

    public void Shuffle(Vector3Int[] arr)
    {
        for (int i = arr.Length; i  > 0; i--)
        {
            int index = Random.Range(0, i);
            Vector3Int temp = arr[i];
            arr[i] = arr[index];
            arr[index] = temp;
        }
    }
}
