using UnityEngine;

public class Node
{
    public bool _walkable;
    public Vector3 _mapPosition;

    public int _gCost;
    public int _hCost;
    public Node parent;

    public int _gridPositionX, _gridPositionY;

    // Calculate f cost
    public int GetFCost()
    {
        return _gCost + _hCost;
    }

    // Node constructor
    public Node(bool walkable, Vector3 mapPosition, int x, int y)
    {
        _walkable = walkable;
        _mapPosition = mapPosition;
        _gridPositionX = x;
        _gridPositionY = y;
    }
}
