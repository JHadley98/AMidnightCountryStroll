using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    public Transform sheep, player;

    private GridMap _gridMap;

    private void Awake()
    {
        _gridMap = GetComponent<GridMap>();
    }

    private void Update()
    {
        List<Node> Positions = FindAStarPath(sheep.position, player.position);
    }

    public List<Node> FindAStarPath(Vector3 startPosition, Vector3 destination)
    {
        Node startNode = _gridMap.FindNodeInGrid(startPosition);
        Node destinationNode = _gridMap.FindNodeInGrid(destination);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Set currentNode to be the first element in the openSet
            Node currentNode = openSet[0];

            // Loop through all nodes in openSet
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i]._hCost < currentNode._hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == destinationNode)
            {
                return RetracePath(startNode, destinationNode);
            }

            // Loop through neighbours
            foreach (Node neighbour in _gridMap.GetNeighbouringNodes(currentNode))
            {
                if (closedSet.Contains(neighbour) || !neighbour._walkable)
                {
                    continue;
                }

                // Calculate new movement cost
                int newMovementCostToNeighbour = currentNode._gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour._gCost || !openSet.Contains(neighbour))
                {
                    // Calculate new gCost
                    neighbour._gCost = newMovementCostToNeighbour;
                    // Calculate new hCost
                    neighbour._hCost = GetDistance(neighbour, destinationNode);
                    // Set parent of the neighbour to the currentNode
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    // RetracePath function used to find the path between the nodes
    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // Retrace steps in path until startNode is reached
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        // Draw path to scene
        _gridMap.path = path;

        return path;
    }

    // Get distance between any 2 given nodes
    private int GetDistance(Node firstNode, Node secondNode)
    {
        int distX = Mathf.Abs(firstNode._gridPositionX - secondNode._gridPositionX);
        int distY = Mathf.Abs(firstNode._gridPositionY - secondNode._gridPositionY);

        if (distX > distY)
        {
            return (4 * distY) + (10 * distX);
        }

        return (4 * distX) + (10 * distY);
    }
}
