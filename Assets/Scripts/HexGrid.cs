using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    public HexCell CenterHex;
    public int OuterRadius; // the absolute value of highest value a xyz coordinate can be, determines the size of the structure

    public HexGrid(int radius)
    {
        OuterRadius = radius;
        // phase 1: generate all hex cells
        Dictionary<Vector3, HexCell> cells = new Dictionary<Vector3, HexCell>();
        for (int x = -OuterRadius; x <= OuterRadius; x++)
        {
            for (int y = -OuterRadius; y <= OuterRadius; y++)
            {
                for (int z = -OuterRadius; z <= OuterRadius; z++)
                {
                    if(x + y + z == 0) // only get cells that will be part of the hex grid
                    {
                        cells.Add(new Vector3(x, y, z), new HexCell(x, y, z));
                    }
                }
            }
        }
        // phase 2: connect all hex cells
        cells.TryGetValue(new Vector3(0, 0, 0), out CenterHex);
        for (int x = -OuterRadius; x <= OuterRadius; x++)
        {
            for (int y = -OuterRadius; y <= OuterRadius; y++)
            {
                for (int z = -OuterRadius; z <= OuterRadius; z++)
                {
                    if (x + y + z == 0) // only get cells that will be part of the hex grid
                    {
                        HexCell currentHex;
                        cells.TryGetValue(new Vector3(x, y, z), out currentHex);
                        HexCell[] neighboringHexes = new HexCell[6];
                        cells.TryGetValue(new Vector3(x, y - 1, z + 1), out neighboringHexes[0]); // top right
                        cells.TryGetValue(new Vector3(x + 1, y - 1, z), out neighboringHexes[1]); // right
                        cells.TryGetValue(new Vector3(x + 1, y, z - 1), out neighboringHexes[2]); // bottom right
                        cells.TryGetValue(new Vector3(x, y + 1, z - 1), out neighboringHexes[3]); // bottom left
                        cells.TryGetValue(new Vector3(x - 1, y + 1, z), out neighboringHexes[4]); // left
                        cells.TryGetValue(new Vector3(x - 1, y, z + 1), out neighboringHexes[5]); // top left
                        currentHex.SetAllSurroundingCells(neighboringHexes);
                    }
                }
            }
        }
    }
}
