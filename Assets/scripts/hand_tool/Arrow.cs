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
    private Vector3 pos, rot, oth;

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

    void Update()
    {
        if (other != null)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            if (stucked  && oth.x - other.transform.rotation.eulerAngles.x > 0.001)
            {
                oth.x = -(oth.x - other.transform.rotation.eulerAngles.x);
                oth.y = -(oth.y - other.transform.rotation.eulerAngles.y);
                oth.z = -(oth.z - other.transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Euler(rot.x + oth.x, rot.y + oth.y, rot.z + oth.z);
                rot.x = transform.rotation.eulerAngles.x - rot.x;
                rot.y = transform.rotation.eulerAngles.y - rot.y;
                rot.z = transform.rotation.eulerAngles.z - rot.z;
            }
            transform.position = other.transform.position - pos;
        }
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
            if (collideNumber == 1)
            {
                this.other = other;
                if (other.transform.tag != "Ground")
                {
                    other.SendMessage("applyDamage", damage); //will be used after we'll write hp-script
                    other.SendMessage("addToColliders", gameObject); //add this arrow to block's colliders list
                }
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().isKinematic = true;
                pos = other.transform.position - transform.position;
                rot.x = transform.rotation.eulerAngles.x;
                rot.y = transform.rotation.eulerAngles.y;
                rot.z = transform.rotation.eulerAngles.z;
                oth.x = other.transform.rotation.eulerAngles.x;
                oth.y = other.transform.rotation.eulerAngles.y;
                oth.z = other.transform.rotation.eulerAngles.z;
                stucked = true;
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
