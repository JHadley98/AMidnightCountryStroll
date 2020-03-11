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

public class Nodes
{
    // Bool walkable is the node walkable or not
    public bool Walkable;
    // Set nodes world position
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;
    public Nodes parent;

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
}
