using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public LayerMask _obstacle;
    [SerializeField] private Vector2 _worldSize = new Vector2();
    private readonly float _nodeSize = 1.0f;
    Node[,] _gridMap;

    private int _gridSizeX, _gridSizeY;

    private void Start()
    {
        // Calculate how many nodes can fit across the world size
        _gridSizeX = Mathf.RoundToInt(_worldSize.x / _nodeSize);
        _gridSizeY = Mathf.RoundToInt(_worldSize.y / _nodeSize);

        CreateGrid();
    }

    private void CreateGrid()
    {
        _gridMap = new Node[_gridSizeX, _gridSizeY];
        // Get bottom left of world, .forward is used to get the Z axis in 3D space
        Vector3 bottomLeft = transform.position - Vector3.right * _worldSize.x / 2 - Vector3.forward * _worldSize.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                // Get each point a node will occupy in the world
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeSize + (_nodeSize / 2)) + Vector3.forward * (y * _nodeSize + (_nodeSize / 2));
                // Collision check
                bool walkable = !Physics.CheckSphere(worldPoint, _nodeSize / 2, _obstacle);
                // Create new node
                _gridMap[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbouringNodes(Node node)
    {
        List<Node> neighbouringNodes = new List<Node>();

        // Loop search in a 3x3 block
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                // Find neighouring positions on x and y axis
                int neighbourX = node._gridPositionX + x;
                int neighbourY = node._gridPositionY + y;

                // Check if neighbour is in grid
                if (neighbourX >= 0 && neighbourX < _gridSizeX && neighbourY >= 0 && neighbourY < _gridSizeY)
                {
                    neighbouringNodes.Add(_gridMap[neighbourX, neighbourY]);
                }
            }
        }

        return neighbouringNodes;
    }

    public Node FindNodeInGrid(Vector3 gridPosition)
    {
        // Convert world position into a percentage for both x and y coordinate, to see how far along the node is in the grid
        float percentageX = (gridPosition.x / _worldSize.x) + 0.5f;
        float percentageY = (gridPosition.z / _worldSize.y) + 0.5f;

        // Clamp percentage between 0 and 1
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);

        // Get x and y indices of the grid array
        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentageX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentageY);

        return _gridMap[x, y];
    }

    // List of nodes used for visualising path in gizmos
    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_worldSize.x, 1, _worldSize.y));

        if (_gridMap != null)
        {
            foreach (Node node in _gridMap)
            {
                Gizmos.color = node._walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node._mapPosition, Vector3.one * (_nodeSize - 0.1f));
            }
        }
    }
}
