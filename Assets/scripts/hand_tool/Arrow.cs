using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool stucked;
    public float damage, velocity;
    private bool collideInd;

    private void Start()
    {
        collideInd = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshCollider>().enabled = true;

        stucked = false;
        
        GetComponent<Rigidbody>().velocity=transform.forward*velocity; //fly
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stucked)
            GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        /*else
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        stucked = true;
        if (other.transform.tag == "Buildable" && !collideInd)
        {
            collideInd = true;
            other.SendMessage("applyDamage", damage); //will be used after we'll write hp-script
            //Debug.Log(damage);
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<MeshCollider>().isTrigger = false; // if you want to collide with it after shoot
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exit");
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshCollider>().enabled = true;
    }
}
