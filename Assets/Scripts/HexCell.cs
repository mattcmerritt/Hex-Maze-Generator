using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    // position
    private int x, y, z;

    // references to surrounding cells
    private HexCell TopLeft, TopRight, Left, Right, BottomLeft, BottomRight;

    // contents of cell
    private bool isOpen;

    // constructor
    public HexCell(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // connecting a cell to this cell
    // position is 0-5, starting with TopRight and going clockwise
    public void SetSurroundCell(int position, HexCell cell)
    {
        switch (position)
        {
            case 0:
                TopRight = cell;
                break;
            case 1:
                Right = cell;
                break;
            case 2:
                BottomRight = cell;
                break;
            case 3:
                BottomLeft = cell;
                break;
            case 4:
                Left = cell;
                break;
            case 5:
                TopLeft = cell;
                break;
            default:
                // shouldn't get here
                break;
        }
    }

    // assumes that six cells (or null references were given)
    // cells should be [TopRight, Right, BottomRight, BottomLeft, Left, TopLeft]
    public void SetAllSurroundingCells(HexCell[] cells)
    {
        if (cells.Length == 6)
        {
            TopRight = cells[0];
            Right = cells[1];
            BottomRight = cells[2];
            BottomLeft = cells[3];
            Left = cells[4];
            TopLeft = cells[5];
        }
    }

    // gives the cells position
    public Vector3 GetPosition() 
    {
        return new Vector3(x, y, z);
    }

    // returns surrounding cells starting at the top and going clockwise
    public HexCell[] GetSurroundingCells()
    {
        return new HexCell[] {TopRight, Right, BottomRight, BottomLeft, Left, TopLeft};
    }
}
