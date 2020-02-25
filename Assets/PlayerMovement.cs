using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public int Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.Translate(transform.right * (Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime));
            transform.Translate(transform.forward * (Input.GetAxisRaw("Vertical") * Speed * Time.deltaTime));
        }
    }
        
}
