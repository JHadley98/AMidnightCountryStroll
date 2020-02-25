using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float Pitch;
    float Yaw;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Yaw += Input.GetAxisRaw("Mouse X");
        Pitch -= Input.GetAxisRaw("Mouse Y");
        transform.eulerAngles = new Vector3(Pitch, Yaw, 0.0f);
        transform.parent.Rotate(0, Input.GetAxisRaw("Mouse X") * 0.5f, 0);
    }
}
