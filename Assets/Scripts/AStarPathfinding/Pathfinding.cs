/* 
 * James Hadley 
 * T7124647
 * Group: The Spanish Inquisition
 * AI for Games Engines (GAV3005-N-FJ1-2019)
 * Teesside University
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    // Reference GameGrid
    private GameGrid grid;
    
    private void Awake()
    {
        // GetComponent GameGrid script
        grid = GetComponent<GameGrid>();
    }

    private void Update()
    {
        FindAStarPath(seeker.position, target.position);
    }

    void FindAStarPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Nodes startNode = grid.NodesFromWorld(startPosition);
        Nodes targetNode = grid.NodesFromWorld(targetPosition);

        // Create heap of nodes for openSet nodes - nodes that are to evaluated
        Heap<Nodes> openSet = new Heap<Nodes>(grid.MaxSize);
        // Create HashSet closedSet of nodes - nodes that are already evaluated
        HashSet<Nodes> closedSet = new HashSet<Nodes>();
        // Add startNode to openSet
        openSet.Add(startNode);

        // While openSet is not empty then count is greater than 0, keep looping
        while (openSet.Count > 0)
        {
            // currentNode is equal to the first element of the openSet
            Nodes currentNode = openSet.RemoveFirstItem();

            closedSet.Add(currentNode);

            // If currentNode equals targetNode then the path is found
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            // Loop through all the neighbouring nodes
            foreach (Nodes neighbourNode in grid.GetNeighbourNode(currentNode))
            {
                // Check if neighbour is walkable or closed then skip to next neighbour
                if(!neighbourNode.Walkable || closedSet.Contains(neighbourNode))
                {
                    continue;
                }   
                
                int newMovementCostToNeighbour = currentNode.G_cost + GetDistance(currentNode, neighbourNode);
                // if new movement cost is less than current G_cost of the neighbout or if the neighbour is not in the openSet then:
                if (newMovementCostToNeighbour < neighbourNode.G_cost || !openSet.Contains(neighbourNode))
                {
                    // Set G_cost equal to newMovementCostToNeighbour which is the new G_cost
                    neighbourNode.G_cost = newMovementCostToNeighbour;
                    // Set H_cost equal to GetDistnance setting neighbourNode and targetNode, calculating the distance for the h cost
                    neighbourNode.H_cost = GetDistance(neighbourNode, targetNode);
                    // Set neighbourNode parent to equal the currentNode
                    neighbourNode.parent = currentNode;

                    // Check if neighbourNode is not in openSet
                    if (!openSet.Contains(neighbourNode))
                    { 
                        // Add neighbourNode to openSet
                        openSet.Add(neighbourNode);
                    }
                }
            }
        }
    }

    void RetracePath(Nodes startNodes, Nodes endNodes)
    {
        // Create list of nodes labelled path
        List<Nodes> path = new List<Nodes>();
        Nodes currentNode = endNodes;

        // Retrace steps until we reach the starting node
        while (currentNode != startNodes)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }


    int GetDistance(Nodes nodeA, Nodes nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            // 14 is the cost of each diagonal move
            return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
    }
}
