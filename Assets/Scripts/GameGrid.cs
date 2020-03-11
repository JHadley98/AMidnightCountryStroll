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

public class GameGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    
    // Vector2 array set world size
    public Vector2 gridWorldSize;
    
    // node radius float variable to define how much space each individual node covers
    public float nodeRadius;
    
    // 2d array of nodes called grid
    Nodes[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        // Calculate how many nodes that can be fit across the grid
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public Nodes NodesFromWorld(Vector3 worldPosition)
    {
        // Convert X coordinate world position into a perentage
        // Used to see how far long the node is across the grid
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;

        // Convert Y coordinate world position into a perentage
        // Used to see how far long the node is across the grid
        // Set worldPosition along the z axis due to 3d array
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        // Clamp between 0 and 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // X and Y indices of the 2d grid array
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    // CreateGrid method
    void CreateGrid()
    {
        grid = new Nodes[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        // Loop through all positions that the grids will be in to check collisions to see if they are walkable or not
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // Collision check
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                // Populate grid with new nodes
                grid[x, y] = new Nodes(walkable, worldPoint, x, y);
            }
        }
    }
  
    // Public list to get neighbouring node
    public List<Nodes> GetNeighbourNode(Nodes nodes)
    {
        // List of nodes for neighbourNode
        List<Nodes> neighbourNode = new List<Nodes>();

        // Loop to search in a 3x3 block
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Both x & y are equal to 0, then skip iteration as it is the node given 
                if (x == 0 && y == 0)
                { 
                    continue;
                }

                int checkX = nodes.gridX + x;
                int checkY = nodes.gridY + y;
                
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbourNode.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbourNode;
    }

    public List<Nodes> path;

    // Visualise grid world size using OnDrawGizmos method
    private void OnDrawGizmos()
    {
        // Draw grid world size
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            // Nodes n in grid
            foreach (Nodes n in grid)
            {
                // Assign gizmos colour, if there is no collision draw cubes white, if there is a collision draw the cubes red
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    { 
                        Gizmos.color = Color.black;
                    }
                }
                    
                // Draw cube, assigning centre and size
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}