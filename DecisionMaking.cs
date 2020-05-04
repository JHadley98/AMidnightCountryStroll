using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaking : MonoBehaviour
{
    public GameObject uiObjectLoose;
    public GameObject uiObjectWin;
    GameObject[] Sheep;
    

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
        
        foreach(GameObject sheep in Sheep)
        { 
            if (Vector3.Distance(sheep.transform.position, GameObject.Find("Player").transform.position) <= 3)//check for close distance between player and sheep
            {
                loose();//when the sheep get too close to the player
                break;
            }
            
            }
        
               
            if (Vector3.Distance(GameObject.Find("Player").transform.position, GameObject.Find("Path").transform.position) <= 4)
            {
                win(); //when player is at the end position
            }
        

    }
    

    void win()
    {
        uiObjectWin.SetActive(true); //game is won

        new WaitForSeconds(10);
        Application.Quit();
        
    }
    

    void loose()
    {
        uiObjectLoose.SetActive(true); //game is lost
        new WaitForSeconds(10);
        Application.Quit();
        
    }


}


