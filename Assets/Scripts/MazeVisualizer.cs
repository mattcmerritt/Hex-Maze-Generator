using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeVisualizer : MonoBehaviour
{
    public HexGrid Grid;
    public Maze Maze;
    public GameObject EmptyHex, FullHex, EmptyHexWithBorder;
    private const float HorizShift = 2.5f;
    private const float VertShift = 2.15f;
    private const float HorizOffset = 1.25f;
    public Camera MainCamera;
    private float CenterX;
    private float CenterY;
    public bool UseBorder;
    public int Radius;
    public Slider RadiusSlider;

    private void Start()
    {
        DrawNewMaze();
    }

    void Update()
    {
        // check slider for maze radius
        Radius = (int) RadiusSlider.value;

        if (Input.GetKeyDown(KeyCode.R))
        {
            DrawNewMaze();
        }
    }

    public void CreateAllHexes()
    {
        // set up the maze
        Maze = GetComponent<Maze>();
        if (Maze == null)
        {
            Debug.LogError("NO MAZE FOUND");
        }
        Grid = Maze.GenerateMaze(Radius);

        DisplayMaze();

        // move camera to center of maze and zoom out
        MoveCameraToCenterHex();
    }

    public void DisplayMaze()
    {
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
                            if (UseBorder)
                            {
                                GameObject tile = Instantiate(EmptyHexWithBorder, new Vector3(positionX, positionY, 0f), Quaternion.identity);
                                if (Maze.StartPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.green;
                                }
                                else if (Maze.EndPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.red;
                                }
                            }
                            else
                            {
                                GameObject tile = Instantiate(EmptyHex, new Vector3(positionX, positionY, 0f), Quaternion.identity);
                                if (Maze.StartPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.green;
                                }
                                else if (Maze.EndPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.red;
                                }
                            }
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
    }

    // move the camera to the center hex using the stored coordinates from generation
    public void MoveCameraToCenterHex()
    {
        MainCamera.transform.position = new Vector3(CenterX, CenterY, -10f);
        MainCamera.orthographicSize = Grid.OuterRadius * 3f;
        if(MainCamera.orthographicSize < 20f)
        {
            MainCamera.orthographicSize = 20f;
        }
    }

    // destroy all hexes on the screen still
    public void DestroyAllHexes()
    {
        GameObject[] hexes = GameObject.FindGameObjectsWithTag("Hex");
        for (int i = 0; i < hexes.Length; i++)
        {
            Destroy(hexes[i]);
        }
    }

    public void UpdateRadius(float sliderInput)
    {
        Radius = (int) sliderInput;
        DrawNewMaze();
    }

    // toggle border for empty hexes
    public void ToggleBorder(bool border)
    {
        UseBorder = border;
        DestroyAllHexes();
        DisplayMaze();
    }

    public void DrawNewMaze()
    {
        DestroyAllHexes();
        CreateAllHexes();
    }
}
