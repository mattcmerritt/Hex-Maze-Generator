using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionWithDepth
{
    public Vector3Int Position;
    public int Depth;

    public PositionWithDepth(Vector3Int position, int depth)
    {
        this.Position = position;
        this.Depth = depth;
    }
}
