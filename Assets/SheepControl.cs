using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepControl : MonoBehaviour
{
    public Transform Player;

    public int FlockAffectDistance;
    public bool ActiveSeparation;
    public bool ActiveAlignment;
    public bool ActiveCohesion;

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
        Acceleration = Vector3.zero;
        if(!Alerted)
        {
            //transform.rotation = Quaternion.LookRotation(Player.transform.position - transform.position, Vector3.up);
            if ((Player.position - transform.position).magnitude <= AlertDistance)
                Alerted = true;
        }
        else
        {
            print(transform.position);
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
                        

                    CohesionEffect += thisSheep.transform.position;
                    NumAffecting++;

                    if((thisSheep.transform.position - transform.position).magnitude < CrowdRadius && (thisSheep.transform.position - transform.position).magnitude > 0)
                    {
                        Vector3 Difference = transform.position - thisSheep.transform.position;
                        Difference.Normalize();
                        Difference /= (thisSheep.transform.position - transform.position).magnitude;
                        SeparationEffect += Difference;
                    }

                    AlignmentEffect += new Vector3(thisSheep.transform.forward.x, 0, thisSheep.transform.forward.z);
                }

            }

            if (ActiveCohesion)
            {
                CohesionEffect -= transform.position;
                CohesionEffect = (CohesionEffect / NumAffecting) ;
                Acceleration += HeadToward(CohesionEffect);
            }
            if (ActiveSeparation)
            {
                Acceleration += HeadToward(SeparationEffect);
            }
            if(ActiveAlignment)
            {
                AlignmentEffect /= NumAffecting;
                Acceleration += HeadToward(AlignmentEffect);
            }

            Velocity += Acceleration.normalized * Time.deltaTime;
            Forward = Velocity.normalized;
            Velocity *= Speed;
            transform.forward = Forward;
            transform.position += Velocity * Time.deltaTime;
        }
        


    }

    Vector3 HeadToward(Vector3 vector)
    {
        Vector3 updatedVector = vector.normalized * Speed - Velocity;
        return Vector3.ClampMagnitude(updatedVector, 10);
    }

    
}
