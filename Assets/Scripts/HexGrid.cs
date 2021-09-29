using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexGrid
{
    public Dictionary<Vector3Int, HexCell> Grid;
    public int OuterRadius; // the absolute value of highest value a xyz coordinate can be, determines the size of the structure

    public HexGrid(int radius)
    {
        OuterRadius = radius;
        // phase 1: generate all hex cells
        Grid = new Dictionary<Vector3Int, HexCell>();
        for (int x = -OuterRadius; x <= OuterRadius; x++)
        {
            for (int y = -OuterRadius; y <= OuterRadius; y++)
            {
                for (int z = -OuterRadius; z <= OuterRadius; z++)
                {
                    if(x + y + z == 0) // only get cells that will be part of the hex grid
                    {
                        Grid.Add(new Vector3Int(x, y, z), new HexCell(x, y, z));
                    }
                }
            }
        }
        // phase 2: connect all hex cells
        for (int x = -OuterRadius; x <= OuterRadius; x++)
        {
            for (int y = -OuterRadius; y <= OuterRadius; y++)
            {
                for (int z = -OuterRadius; z <= OuterRadius; z++)
                {
                    if (x + y + z == 0) // only get cells that will be part of the hex grid
                    {
                        HexCell currentHex;
                        Grid.TryGetValue(new Vector3Int(x, y, z), out currentHex);
                        HexCell[] neighboringHexes = new HexCell[6];
                        Grid.TryGetValue(new Vector3Int(x, y - 1, z + 1), out neighboringHexes[0]); // top right
                        Grid.TryGetValue(new Vector3Int(x + 1, y - 1, z), out neighboringHexes[1]); // right
                        Grid.TryGetValue(new Vector3Int(x + 1, y, z - 1), out neighboringHexes[2]); // bottom right
                        Grid.TryGetValue(new Vector3Int(x, y + 1, z - 1), out neighboringHexes[3]); // bottom left
                        Grid.TryGetValue(new Vector3Int(x - 1, y + 1, z), out neighboringHexes[4]); // left
                        Grid.TryGetValue(new Vector3Int(x - 1, y, z + 1), out neighboringHexes[5]); // top left
                        currentHex.SetAllSurroundingCells(neighboringHexes);
                    }
                }
            }
        }
    }

    public HexCell GetHexAtPosition(int x, int y, int z)
    {
        HexCell currentHex;
        Grid.TryGetValue(new Vector3Int(x, y, z), out currentHex);
        return currentHex;
    }
}
