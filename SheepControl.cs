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
        print(Sheep.Length);
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
            //transform.rotation = Quaternion.LookRotation(Player.transform.position - transform.position, Vector3.up);
            if ((Player.position - transform.position).magnitude <= AlertDistance)
                Alerted = true;
        }
        else
        {
            Vector3 SeparationEffect = Vector3.zero;
            Vector3 AlignmentEffect = Vector3.zero;
            Vector3 CohesionEffect = Vector3.zero;
            int NumAffecting = -1;
            

            foreach (GameObject thisSheep in Sheep)
            {
                if ((thisSheep.transform.position - transform.position).magnitude < FlockAffectDistance)
                {
                    SheepControl otherSheepControl = thisSheep.GetComponent<SheepControl>();
                    if (Alerted && !otherSheepControl.Alerted)
                        otherSheepControl.Alerted = true;
                        

                    CohesionEffect += new Vector3(thisSheep.transform.position.x, 0, thisSheep.transform.position.z);
                    
                    NumAffecting++;
                    

                    //Separation Code Begins
                    if((thisSheep.transform.position - transform.position).magnitude < CrowdRadius && (thisSheep.transform.position - transform.position).magnitude > 0)
                    {
                        Vector3 Difference = transform.position - thisSheep.transform.position;

                        Difference.Normalize();
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
                if((CohesionEffect - transform.position).magnitude > MinCohesionDist)
                    Acceleration += (CohesionEffect - transform.position);
            }
            if (ActiveSeparation)
            {
                Acceleration += SeparationEffect * SeparationStrength;
            }
            if(ActiveAlignment)
            {
                //AlignmentEffect /= NumAffecting;
                Acceleration += (AlignmentEffect.normalized - Acceleration);
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
