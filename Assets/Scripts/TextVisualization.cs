using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVisualization : MonoBehaviour
{
    public HexGrid Grid;
    public int GridRadius;
    public string Output;

    void Start()
    {
        Grid = new HexGrid(GridRadius);
        Output = "";
        for (int x = -Grid.OuterRadius; x <= Grid.OuterRadius; x++)
        {
            for (int y = -Grid.OuterRadius; y <= Grid.OuterRadius; y++)
            {
                for (int z = Grid.OuterRadius; z >= -Grid.OuterRadius; z--)
                {
                    if (x + y + z == 0) // only get cells that will be part of the hex grid
                    {
                        if(Grid.GetHexAtPosition(x, y, z).isOpen)
                        {
                            Output += "O";
                        }
                        else
                        {
                            Output += "X";
                        }
                    }
                }
            }
            Output += "\n";
        }
        Debug.Log(Output);
    }
}
