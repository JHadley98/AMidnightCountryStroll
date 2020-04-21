using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepControl : MonoBehaviour
{
    public Transform Player;


    
    public int FlockAffectDistance;
    public bool ActiveAlignment;
    public bool ActiveCohesion;
    [Range(0, 10)]
    public int MinCohesionDist;
    public bool ActiveSeparation;
    [Range(1, 10)]
    public int SeparationStrength;

    Vector3 Velocity;
    Vector3 Acceleration;
    Vector3 Forward;

    [Range(1,10)]
    public int Speed;

    public int AlertDistance;
    public bool Alerted = false;
    public float CrowdRadius;
    GameObject[] Sheep;
    // Start is called before the first frame update
    void Start()
    {
        Sheep = GameObject.FindGameObjectsWithTag("Sheep");
        //print(Sheep.Length);
        Forward = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            SeparationStrength++;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SeparationStrength--;
        }

        Acceleration = Vector3.zero;
        if(!Alerted)
        {
            //Makes sheep look at player while unalerted
            transform.rotation = Quaternion.LookRotation(Player.transform.position - transform.position, Vector3.up);

            //Detect when a player alerts the sheep
            if ((Player.position - transform.position).magnitude <= AlertDistance)
                Alerted = true;
        }
        else
        {
            //Resetting flocking effects
            Vector3 SeparationEffect = Vector3.zero;
            Vector3 AlignmentEffect = Vector3.zero;
            Vector3 CohesionEffect = Vector3.zero;
            int NumAffecting = 0;
            
            
            foreach (GameObject thisSheep in Sheep)
            {
                //Check to see that sheep the current sheep in the loop is within the effecting range of this sheep
                if ((thisSheep.transform.position - transform.position).magnitude < FlockAffectDistance)
                {
                    SheepControl otherSheepControl = thisSheep.GetComponent<SheepControl>();
                    //Alerting other sheep if it was not before
                    if (Alerted && !otherSheepControl.Alerted)
                        otherSheepControl.Alerted = true;
                        
                    //Collecting current sheeps position data to find the average/mid position of the effecting sheep
                    CohesionEffect += new Vector3(thisSheep.transform.position.x, 0, thisSheep.transform.position.z);
                    
                    //Keep track of how many sheep are effecting this sheep
                    NumAffecting++;
                    

                    //Separation Code Begins
                    if((thisSheep.transform.position - transform.position).magnitude < CrowdRadius && (thisSheep.transform.position - transform.position).magnitude > 0)
                    {
                        //Calculating the difference in position of this and the current loop sheep
                        Vector3 Difference = transform.position - thisSheep.transform.position;

                        Difference.Normalize();
                        //Dividing the difference by the squared distance between the 2 sheep so the separation effect will be greater when theyre closer together
                        Difference /= (thisSheep.transform.position - transform.position).magnitude * (thisSheep.transform.position - transform.position).magnitude;
                        SeparationEffect += Difference;

                    }
                    //Separation Code Ends

                    AlignmentEffect += new Vector3(thisSheep.transform.forward.x, 0, thisSheep.transform.forward.z);
                }

            }
            if (ActiveCohesion)
            {
                //CohesionEffect -= transform.position;

                CohesionEffect = (CohesionEffect / NumAffecting);
                if ((CohesionEffect - transform.position).magnitude > MinCohesionDist)
                    Acceleration += (CohesionEffect - transform.position);
            }
            if (ActiveSeparation)
            {
                Acceleration += SeparationEffect * SeparationStrength;
            }
            if(ActiveAlignment)
            {
                AlignmentEffect /= NumAffecting;
                Acceleration += (AlignmentEffect - Acceleration)/2;
            }

            Velocity = Acceleration.normalized;
            Velocity.y = 0;
            if(Velocity != Vector3.zero)
                Forward = Velocity.normalized;
            transform.forward = Forward;
            transform.position += Forward * Speed * Time.deltaTime;
        }
        


    }

    
}
