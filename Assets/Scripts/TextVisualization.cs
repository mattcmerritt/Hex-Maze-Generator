using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVisualization : MonoBehaviour
{
    public HexGrid Grid;
    public int GridRadius;
    private string Output;

    void Start()
    {
        // set up an empty hex grid
        Grid = new HexGrid(GridRadius);

        // change the rightmost hex and hex top left from center to open
        Grid.GetHexAtPosition(3, -3, 0).isOpen = true;
        Grid.GetHexAtPosition(-1, 0, 1).isOpen = true;

        // print the hex grid
        Output = "";
        int SpaceBuffer = GridRadius;

        for (int z = Grid.OuterRadius; z >= -Grid.OuterRadius; z--)
        {
            for (int i = Mathf.Abs(SpaceBuffer); i > 0; i--)
            {
                Output += "  ";
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
