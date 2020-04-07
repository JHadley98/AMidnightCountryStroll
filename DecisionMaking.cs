using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(GameObject.Find("Player").transform.position, GameObject.Find("Sheep").transform.position) <= 7)
        {
            chase();
        }
        else
        {
            sleep();
        }

        if (Vector3.Distance(GameObject.Find("Player").transform.position,GameObject.Find("Sheep").transform.position) <= 2)
        {
            attack();
        }
    }

    void sleep()
    {
        print("sleepy sheep");
        
    }

    void chase()
    {
        print("sheep chasing");
        //gameObject.GetComponent<SheepControl>();?

    }

    void attack()
    {
        print("sheep attacking");
    }


}


