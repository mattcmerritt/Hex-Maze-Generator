using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    // position
    public int x;
    public int y;
    public int z;

    // references to surrounding cells
    public HexCell topLeft, topRight, left, right, bottomLeft, bottomRight;
}
