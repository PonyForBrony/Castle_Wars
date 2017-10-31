using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool stucked;
    public float damage, velocity;
    private int collideNumber;

    private void Start()
    {
        collideNumber = 0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshCollider>().enabled = true;

        stucked = false;

        GetComponent<Rigidbody>().velocity = transform.forward * velocity; //fly
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stucked)
            GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
       /* else
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Fallen" && other.transform.tag != "Untagged" && !stucked)
        {
            collideNumber++;
            stucked = true;
            Debug.Log("TriggerEnter   " + other.transform.tag + "   " + other.transform.name + "   " + collideNumber);
            if (collideNumber == 1)
            {
                if (other.transform.tag != "Ground")
                {
                    other.SendMessage("applyDamage", damage); //will be used after we'll write hp-script
                    other.SendMessage("addToColliders", gameObject); //add this arrow to block's colliders list
                }
                //Debug.Log(damage);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().isKinematic = true;
                //transform.parent = other.transform;
                //transform.localScale.Set(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            }
        }
        //GetComponent<Renderer>().material.color = new Color(collideNumber, collideNumber, collideNumber);
    }

    public void OnParentDestroy(Collider other)
    {
        Debug.Log("TriggerExit   " + other.transform.tag + "   " + other.transform.name + "   " + collideNumber);
        if (other.transform.tag != "Fallen" && other.transform.tag != "Untagged")
        {
            collideNumber--;
            Debug.Log("TriggerExit2   " + other.transform.tag + "   " + other.transform.name + "   " + collideNumber);
            if (collideNumber == 0)
            {
                GetComponent<MeshCollider>().enabled = true;
                GetComponent<MeshCollider>().isTrigger = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        //GetComponent<Renderer>().material.color = new Color(collideNumber, collideNumber, collideNumber);
    }
}
