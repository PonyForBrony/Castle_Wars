using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builded : MonoBehaviour
{
    public Material blockMaterial;
    private Vector3 inCastlePos;
    private string name;
    public bool canBuildOnTop, canBuildOnBottom, canBuildOnFront, canBuildOnBack, canBuildOnLeft, canBuildOnRight,isChekedForCloud;
    public List<GameObject> colliders;

    // Use this for initialization
    void Start() // set new characteristic for builded cube or other object
    {
        name = gameObject.name;
        GetComponent<Renderer>().material = blockMaterial;
        gameObject.layer = 0;
        transform.tag = "Buildable";
        GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void setFallen()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.tag = "Fallen";  // tag for ignore fallen cube
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

    public string getPrefabName()
    {
        return name;
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

    private void notAPIOnDestroy()
    {
        if (this.enabled)
        {
            foreach (var item in colliders)
            {
                item.GetComponent<Arrow>().OnParentDestroy(GetComponent<Collider>());
            }

            transform.parent.SendMessage("checkForBuildClouds", inCastlePos);
        }
    }
}
