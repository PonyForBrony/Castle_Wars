using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool stucked;
    public float damage, velocity;

    public float timeForFallingArrow, timeForStucketArrow, destroyTime;
    private float hitTime;
    private Collider other;

    Color color;

    private void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshCollider>().enabled = true;
        stucked = false;

        GetComponent<Rigidbody>().velocity = transform.forward * velocity; //fly
    }

    private void Update()
    {
        if (stucked)
        {
            if (color.a > 0 && (hitTime + timeForStucketArrow) - Time.time < 0)
            {
                color.a = ((hitTime + timeForStucketArrow + destroyTime) - Time.time) / destroyTime;
                if (color.a >= 0)
                    GetComponent<Renderer>().material.color = color;
            }
            else if (color.a <= 0)
            {
                if (other != null && other.transform.tag != "Ground")
                    other.SendMessage("deleteFromColliders", gameObject);
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stucked)
            GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Fallen" && other.transform.tag != "Player" && !stucked)
        {
            Transform handler = other.transform.Find("ChildrenHandler");

            if (handler != null)
                transform.SetParent(handler);
            else
                transform.SetParent(other.transform);

            hitTime = Time.time;
            color = GetComponent<Renderer>().material.color;
            this.other = other;

            if (other.transform.tag != "Ground")
            {
                other.SendMessage("applyDamage", damage); //will be used after we'll write hp-script
                other.SendMessage("addToColliders", gameObject); //add this arrow to block's colliders list
            }
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().isKinematic = true;
            stucked = true;
            hitTime = Time.time;
        }
    }

    public void OnParentDestroy(Collider other)
    {
        /*if (other.transform.tag != "Fallen" && other.transform.tag != "Player")
        {
            transform.parent = null;
            tmp = timeForFallingArrow;
            GetComponent<MeshCollider>().isTrigger = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().isKinematic = false;
        }*/
        Destroy(this);
    }
}
