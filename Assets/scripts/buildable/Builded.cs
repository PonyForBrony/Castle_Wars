using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builded : MonoBehaviour
{
    public Material blockMaterial;
    private Vector3 inCastlePos;

    public void setInCastlePos(Vector3 value)
    {
        inCastlePos = value;
    }

    public Vector3 getInCastlePos()
    {
        return inCastlePos;
    }


    // Use this for initialization
    void Start() // set new characteristic for builded cube or other object
    {
        GetComponent<Renderer>().material = blockMaterial;
        gameObject.layer = 0;
        transform.tag = "Buildable";
        GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

}
