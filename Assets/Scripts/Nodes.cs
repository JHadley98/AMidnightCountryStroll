using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes
{
    public bool walkable;
    public Vector3 worldPosition;

    public Nodes(bool _walkable, Vector3 worldPos)
    {
        walkable = _walkable;
        worldPosition = worldPos;
    }
}
