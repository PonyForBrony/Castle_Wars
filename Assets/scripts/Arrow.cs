using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool stucked;
    public float damage;

    private void Start()
    {
        stucked = false;
        /*for Test!!*/
        GetComponent<Rigidbody>().velocity=transform.forward*15;
        /*for Test!!*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stucked)
            GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        else
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnTriggerEnter(Collider other)
    {
        stucked = true;
        //other.SendMessage("applyDamage",damage); //will be used after we'll write hp-script  
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<MeshCollider>().isTrigger = false;
        GetComponent<MeshCollider>().convex = false;
    }
}
