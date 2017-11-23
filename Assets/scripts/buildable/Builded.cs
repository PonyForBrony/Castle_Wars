using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builded : MonoBehaviour
{

    public Material blockMaterial;
    private Vector3 inCastlePos;

    public bool canBuildOnTop, canBuildOnBottom, canBuildOnFront, canBuildOnBack, canBuildOnRight, canBuildOnLeft;
    public bool[] canBuildOn;
    public bool fallen;

    public List<GameObject> colliders;


    // Use this for initialization
    public void Start() // set new characteristic for builded cube or other object
    {
        fallen = false;
        canBuildOn = new bool[] { canBuildOnTop, canBuildOnBottom, canBuildOnFront, canBuildOnBack, canBuildOnRight, canBuildOnLeft };

        GetComponent<Renderer>().material = blockMaterial;
        gameObject.layer = 0;
        transform.tag = "Buildable";
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<MeshCollider>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (fallen)
            GetComponent<Rigidbody>().WakeUp();
    }

    void setFallen()
    {
        fallen = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.tag = "Fallen";  // tag for ignore fallen cube
        //Destroy(gameObject);
    }


    /*getters & setters*/
    public void setInCastlePos(Vector3 value)
    {
        inCastlePos = value;
    }

    public Vector3 getInCastlePos()
    {
        return inCastlePos;
    }
    /*getters & setters*/

    void addToColliders(GameObject obj)
    {
        colliders.Add(obj);
    }

    void deleteFromColliders(GameObject obj)
    {
        colliders.Remove(obj);
    }

    private void notAPIOnDestroy(bool checkForClouds)
    {
        if (this.enabled)
        {
            foreach (var item in colliders)
            {
                item.GetComponent<Arrow>().OnParentDestroy(GetComponent<Collider>());
            }

            if (checkForClouds)
                transform.parent.SendMessage("checkForBuildClouds", this);
        }
    }

    public int getOppositeSide(int side)
    {
        if (side % 2 == 0)
            return side + 1;
        else
            return side - 1;
    }

    public WritebleView getWritebleView()
    {
        return new WritebleView(name, inCastlePos);
    }
}

[Serializable]
public struct WritebleView
{
    public string name;
    public Vector3 inCastlePos;

    public WritebleView(string name, Vector3 inCastlePos)
    {
        this.name = name;
        this.inCastlePos = inCastlePos;
    }
}
