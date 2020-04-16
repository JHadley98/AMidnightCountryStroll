using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private List<Node> _targets = new List<Node>();
    private int _destinationPoint = 0;
    public float _speed;
    public Vector3 _destination;

    private void Awake()
    {
        // Enable behaviours to be used in the FindPathToTarget update method
        this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _targets = GetComponent<FindPathToTarget>().Path;
        _destination = _targets[_destinationPoint]._mapPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // If Distance between A and B is less than 0.05 then GoToNextPoint on path
        if (Vector3.Distance(_targets[_destinationPoint]._mapPosition, transform.position) < 0.05)
        {
            GoToNextPoint();
        }
    }

    // Method used to set the destination for the AI for follow on a given path between the player and sheep
    private void GoToNextPoint()
    {
        _destinationPoint = (_destinationPoint + 1) % _targets.Count;

        if (_destinationPoint == 0)
        {
            _targets.Reverse();
        }

        _destination = _targets[_destinationPoint]._mapPosition;
    }
}
