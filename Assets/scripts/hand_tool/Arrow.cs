using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool stucked;
    public float damage, velocity;
    private int collideNumber;

    public float timeForFallingArrow, timeForStucketArrow, destroyTime;
    private float tmp;
    private Collider other;

    Color color;

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
        else
        {
            tmp = tmp - Time.deltaTime;
            if (color.a > 0 && tmp < 0)
            {
                color.a = color.a - Time.deltaTime / destroyTime;
                GetComponent<Renderer>().material.color = color;
            }
            else if (color.a <= 0)
            {
                if (other != null)
                    other.SendMessage("deleteFromColliders", gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Fallen" && other.transform.tag != "Player" && !stucked)
        {
            tmp = timeForStucketArrow;
            color = GetComponent<Renderer>().material.color;
            collideNumber++;
            stucked = true;
            if (collideNumber == 1)
            {
                if (other.transform.tag != "Ground")
                {
                    this.other = other;
                    other.SendMessage("applyDamage", damage); //will be used after we'll write hp-script
                    other.SendMessage("addToColliders", gameObject); //add this arrow to block's colliders list
                }
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().isKinematic = true;
                //transform.parent = other.transform;
                //transform.localScale.Set(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            }
        }
    }

    public void OnParentDestroy(Collider other)
    {
        if (other.transform.tag != "Fallen" && other.transform.tag != "Player")
        {
            tmp = timeForFallingArrow;
            collideNumber--;
            if (collideNumber == 0)
            {
                GetComponent<MeshCollider>().isTrigger = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
