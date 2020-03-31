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

public class Nodes : IHeapItem<Nodes>
{
    // Bool walkable is the node walkable or not
    public bool Walkable;
    // Set nodes world position
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;
    public Nodes parent;
    int heapIndex;
    // Distance from starting node
    public int G_cost;
    // H cost (Heuristic) distance from end node
    public int H_cost;

    // Set nodes value constructor
    public Nodes(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        Walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    // Public function calculate F cost
    public int F_cost
    {
        get
        {
            return G_cost + H_cost;
        }
    }

    // HeapIndex method to get the HeapIndex & set the HeapIndex equal to value
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    // CompareTo method used to compare the nodes of the two nodes
    public int CompareTo(Nodes nodesToCompare)
    {
        // Compare F_cost
        int compare = F_cost.CompareTo(nodesToCompare.F_cost);
        // If the 2 F_costs are equal then:
        if (compare == 0)
        {
            // Compare H_cost as a tie breaker
            compare = H_cost.CompareTo(nodesToCompare.H_cost);
        }
        // Return -compare if it is lower than 1
        return -compare;
    }
}
