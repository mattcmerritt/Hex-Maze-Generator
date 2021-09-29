using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public HexGrid Grid;
    private Maze Maze;
    public GameObject EmptyHex, FullHex;
    private const float HorizShift = 2.5f;
    private const float VertShift = 2.15f;
    private const float HorizOffset = 1.25f;
    public Camera MainCamera;
    private float CenterX;
    private float CenterY;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // set up the maze
            Maze = GetComponent<Maze>();
            if (Maze == null)
            {
                Debug.LogError("NO MAZE FOUND");
            }
            Grid = Maze.CurrentMaze;

            float positionX = 0;
            float positionY = 0;
            int lineOffset = Grid.OuterRadius;

            // display the maze
            for (int z = Grid.OuterRadius; z >= -Grid.OuterRadius; z--)
            {
                for (int i = Mathf.Abs(lineOffset); i > 0; i--)
                {
                    positionX += HorizOffset;
                }

                for (int y = Grid.OuterRadius; y >= -Grid.OuterRadius; y--)
                {
                    for (int x = Grid.OuterRadius; x >= -Grid.OuterRadius; x--)
                    {
                        if (x + y + z == 0) // only get cells that will be part of the hex grid
                        {
                            // if at center hex store coordinates for use later
                            if (x == y && y == z && z == 0)
                            {
                                CenterX = positionX;
                                CenterY = positionY;
                            }
                            if (Grid.GetHexAtPosition(x, y, z).isOpen)
                            {
                                Instantiate(EmptyHex, new Vector3(positionX, positionY, 0f), Quaternion.identity);
                            }
                            else
                            {
                                Instantiate(FullHex, new Vector3(positionX, positionY, 0f), Quaternion.identity);
                            }
                            positionX += HorizShift;
                        }
                    }
                }
                positionX = 0;
                positionY += VertShift;
                lineOffset--;
            }

            // move camera to center of maze and zoom out
            MoveCameraToCenterHex();
        }
    }

    // move the camera to the center hex using the stored coordinates from generation
    public void MoveCameraToCenterHex()
    {
        MainCamera.transform.position = new Vector3(CenterX, CenterY, -10f);
        MainCamera.orthographicSize = Grid.OuterRadius * 3f;
    }
}
