using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool stucked;
    public float damage,velocity;

    private void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;

        stucked = false;
        
        GetComponent<Rigidbody>().velocity=transform.forward*velocity; //fly
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stucked)
        {
            GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
            GetComponent<MeshCollider>().enabled = true;
        }
        else
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnTriggerEnter(Collider other)
    {
        stucked = true;
        //other.SendMessage("applyDamage",damage); //will be used after we'll write hp-script
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<MeshCollider>().isTrigger = false; // if you want to collide with it after shoot
        GetComponent<MeshCollider>().convex = false;
    }
}
