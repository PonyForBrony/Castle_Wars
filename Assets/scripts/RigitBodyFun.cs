using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigitBodyFun : MonoBehaviour
{
    Vector3 velocity,angularVelocity;
    
    bool oldP;

    // Use this for initialization
    void Start()
    {
        oldP = false;
        velocity = new Vector3(0, 0, 0);
        angularVelocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (oldP != PhysPouse.on) // enter if "on" was changed
        {
            if (PhysPouse.on) // save vilocity and angularVelocity before stop physics
            {
                velocity = GetComponent<Rigidbody>().velocity;
                angularVelocity = GetComponent<Rigidbody>().angularVelocity;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().isKinematic = true;
            }
            else // give to the cube vilocity and angularVelocity after start physics
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().angularVelocity = angularVelocity;
                GetComponent<Rigidbody>().velocity = velocity;
                transform.tag = "Fallen";  // tag for ignore fallen cube
            }
        }
        oldP = PhysPouse.on;
    }
}
