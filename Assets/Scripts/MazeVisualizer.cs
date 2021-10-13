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
        Radius = (int)RadiusSlider.value;

        if (Input.GetKeyDown(KeyCode.R))
        {
            DrawNewMaze();
        }
    }

    public void CreateAllHexes()
    {
        // set up the maze
        Maze = GetComponent<Maze>();
        Maze.PathList = new List<Vector3Int>();
        Maze.VisitedList = new List<Vector3Int>();
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
                                if (Maze.CurrentPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.blue;
                                }
                                else if (Maze.StartPosition == new Vector3Int(x, y, z))
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
                                if (Maze.CurrentPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.blue;
                                }
                                else if (Maze.StartPosition == new Vector3Int(x, y, z))
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
        if (MainCamera.orthographicSize < 20f)
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
        Radius = (int)sliderInput;
        DrawNewMaze();
    }

    // toggle border for empty hexes
    public void ToggleBorder(bool border)
    {
        UseBorder = border;
        DestroyAllHexes();
        UpdateMazeVisualsAfterMove();
    }

    public void DrawNewMaze()
    {
        DestroyAllHexes();
        CreateAllHexes();
    }

    public void UpdateMazeVisualsAfterMove()
    {
        DestroyAllHexes();

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
                                if (Maze.EndPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.red;
                                }
                                else if (Maze.CurrentPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.blue;
                                }
                                else if (Maze.StartPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.green;
                                }
                                else if (Maze.PathList.Contains(new Vector3Int(x, y, z)))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = new Color(127, 255, 127);
                                }
                                else if (Maze.VisitedList.Contains(new Vector3Int(x, y, z)))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = new Color(127, 127, 255);
                                }
                            }
                            else
                            {
                                GameObject tile = Instantiate(EmptyHex, new Vector3(positionX, positionY, 0f), Quaternion.identity);
                                if (Maze.EndPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.red;
                                }
                                else if (Maze.CurrentPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.blue;
                                }
                                else if (Maze.StartPosition == new Vector3Int(x, y, z))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = Color.green;
                                }
                                else if (Maze.PathList.Contains(new Vector3Int(x, y, z)))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = new Color(127, 255, 127);
                                }
                                else if (Maze.VisitedList.Contains(new Vector3Int(x, y, z)))
                                {
                                    SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                                    renderer.color = new Color(127, 127, 255);
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

    // for some reason, I realized that x is z, y is x, and z is y. Fix later if possible and then correct these values.

    public void MoveCurrentPosition(HexCell newHex)
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        Vector3Int newPos = newHex.GetPosition();
        if (!newHex.isOpen)
        {
            Debug.Log("Hit wall");
            // can't move, do nothing
        }
        else if (newPos == Maze.EndPosition)
        {
            // win message
            currentHex.isCurrent = false;
            Maze.CurrentPosition = newPos;
            newHex.isCurrent = true;
        }
        else if (Maze.PathList.Contains(newPos))
        {
            Debug.Log("Went backwards");
            currentHex.isCurrent = false;
            Maze.PathList.Remove(new Vector3Int(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z));
            Maze.CurrentPosition = newPos;
            newHex.isCurrent = true;
        }
        else if (Maze.VisitedList.Contains(newPos))
        {
            Debug.Log("Hit previously visited tile");
            currentHex.isCurrent = false;
            Maze.PathList.Add(new Vector3Int(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z));
            Maze.CurrentPosition = newPos;
            newHex.isCurrent = true;
        }
        else
        {
            Debug.Log("Hit empty tile");
            currentHex.isCurrent = false;
            Maze.VisitedList.Add(new Vector3Int(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z));
            Maze.PathList.Add(new Vector3Int(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z));
            Maze.CurrentPosition = newPos;
            newHex.isCurrent = true;
        }
        UpdateMazeVisualsAfterMove();
    }

    public void MoveTopRight()
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        HexCell newHex = currentHex.GetSurroundingCells()[0];
        MoveCurrentPosition(newHex);
    }
    public void MoveRight()
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        HexCell newHex = currentHex.GetSurroundingCells()[1];
        MoveCurrentPosition(newHex);
    }
    public void MoveBottomRight()
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        HexCell newHex = currentHex.GetSurroundingCells()[2];
        MoveCurrentPosition(newHex);
    }
    public void MoveBottomLeft()
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        HexCell newHex = currentHex.GetSurroundingCells()[3];
        MoveCurrentPosition(newHex);
    }
    public void MoveLeft()
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        HexCell newHex = currentHex.GetSurroundingCells()[4];
        MoveCurrentPosition(newHex);
    }
    public void MoveTopLeft()
    {
        HexCell currentHex = Maze.CurrentMaze.GetHexAtPosition(Maze.CurrentPosition.x, Maze.CurrentPosition.y, Maze.CurrentPosition.z);
        HexCell newHex = currentHex.GetSurroundingCells()[5];
        MoveCurrentPosition(newHex);
    }
}
