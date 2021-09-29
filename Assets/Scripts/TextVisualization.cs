using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVisualization : MonoBehaviour
{
    public HexGrid Grid;
    private string Output;

    void Start()
    {
        // set up an empty hex grid
        Grid = GetComponent<Maze>().CurrentMaze;

        // print the hex grid
        Output = "";
        int SpaceBuffer = Grid.OuterRadius;

        for (int z = Grid.OuterRadius; z >= -Grid.OuterRadius; z--)
        {
            for (int i = Mathf.Abs(SpaceBuffer); i > 0; i--)
            {
                Output += " ";
            }

            for (int y = Grid.OuterRadius; y >= -Grid.OuterRadius; y--)
            {
                for (int x = Grid.OuterRadius; x >= -Grid.OuterRadius; x--)
                {
                    if (x + y + z == 0) // only get cells that will be part of the hex grid
                    {
                        if(Grid.GetHexAtPosition(x, y, z).isOpen)
                        {
                            Output += "O ";
                        }
                        else
                        {
                            Output += "X ";
                        }
                    }
                }
            }
            Output += "\n";
            SpaceBuffer--;
        }
        Debug.Log(Output);
    }
}
