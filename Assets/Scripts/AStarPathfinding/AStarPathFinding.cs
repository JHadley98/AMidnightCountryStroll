using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    GridMap _gridMap;

    void Awake()
    {
        _gridMap = GetComponent<GridMap>();
    }

    public List<Node> FindAStarPath(Vector3 startPosition, Vector3 destination)
    {
        Node startNode = _gridMap.FindNodeInGrid(startPosition);
        Node destinationNode = _gridMap.FindNodeInGrid(destination);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        openSet.Add(startNode);

        // If player is in a place that is not walkable then return a set with just the startNode, so sheep don't move
        if (!destinationNode._walkable)
        {
            return openSet;
        }

        // If sheep is very close to player then no need to move towards the player.
        if (Vector3.Distance(startPosition, destination) < 3.0f)
        {
            return openSet;
        }

        while (openSet.Count > 0)
        {
            // Set currentNode to be the first element in the openSet
            Node currentNode = openSet[0];

            // Loop through all nodes in openSet
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].GetFCost() < currentNode.GetFCost() || openSet[i].GetFCost() == currentNode.GetFCost() && openSet[i]._hCost < currentNode._hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // If currentNode equals destinationNode return current path
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
        // If all possible paths checked and cannot be reached then return a path which is just the start point, so that sheep don't move
        openSet.Add(startNode);
        return openSet;
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

        Vector3 prevPos = path[0]._mapPosition;
        foreach (Node nextNode in path)
        {
            Debug.DrawLine(prevPos, nextNode._mapPosition, Color.blue);
            prevPos = nextNode._mapPosition;
        }

        return path;
    }

    // Get distance between any 2 given nodes (times 100 to allow for decimal values as result is an int)
    private int GetDistance(Node firstNode, Node secondNode)
    {
        int distX = Mathf.Abs(firstNode._gridPositionX - secondNode._gridPositionX);
        int distY = Mathf.Abs(firstNode._gridPositionY - secondNode._gridPositionY);

        return (int)(Mathf.Sqrt((distX * distX) + (distY * distY)) * 100);
    }
}
