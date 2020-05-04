using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaking : MonoBehaviour
{
    public GameObject uiObjectLoose;
    public GameObject uiObjectWin;
    GameObject[] Sheep;
    //public GameObject script;

    // Start is called before the first frame update
    void Start()
    {
        uiObjectLoose.SetActive(false);
        uiObjectWin.SetActive(false);
        Sheep = GameObject.FindGameObjectsWithTag("Sheep");
    }

    // Update is called once per frame
    void Update()
    {
        //this is not being used but bits of code are incudled in other scripts

        //if (Vector3.Distance(GameObject.Find("Player").transform.position, GameObject.Find("Sheep").transform.position) <= 7)
        //{
        //    chase(); //sheep following player
        //}
        //else
        //{
        //    sleep(); //default state
        //}


        foreach(GameObject sheep in Sheep)
        { 
            if (Vector3.Distance(sheep.transform.position, GameObject.Find("Player").transform.position) <= 3)//check for close distance between player and sheep
            {
                loose();//when the sheep collide with player
                break;
            }
            
            }
        

        
            if (Vector3.Distance(GameObject.Find("Player").transform.position, GameObject.Find("Path").transform.position) <= 4)
            {
                win();
            }
        

    }

    //void sleep()
    //{
    //    print("sleepy sheep");
    //}   


    //void chase()
    //{
    //    print("sheep chasing");
    //gameObject.GetComponent<SheepControl>();?
    //    }

    void win()
    {
        uiObjectWin.SetActive(true);

        new WaitForSeconds(10);
        Application.Quit();
        //script.SetActive(false);
    }
    

    void loose()
    {
        uiObjectLoose.SetActive(true);
        new WaitForSeconds(10);
        Application.Quit();
        //script.SetActive(false);
    }


}


