using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathToTarget : MonoBehaviour
{
    // Use player as target for AI
    public GameObject Player;
    private GameObject _AStarController;
    private AStarPathFinding _pathFinding;

    // Public list node path, get and set Path
    public List<Node> Path { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _AStarController = GameObject.Find("A* Controller");
        _pathFinding = _AStarController.GetComponent<AStarPathFinding>();
    }

    // Update is called once per frame
    void Update()
    {
        Path = _pathFinding.FindAStarPath(gameObject.transform.position, Player.transform.position);

        if (Path != null && !GetComponent<AIController>().enabled)
        {
            GetComponent<AIController>().enabled = true;
        }
    }
}
