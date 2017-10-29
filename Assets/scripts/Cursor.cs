using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Castle castle; 
    public Color tRed, tBlue, tGreen;
    public Material cursorMaterial;
    public Vector3 inCastlePos;

    private bool onSurface;

    // Use this for initialization
    void Start()
    {
        if (castle == null)
            castle = GameObject.Find("CastleHandler").GetComponent<Castle>();
        GetComponent<Renderer>().material = cursorMaterial;
        GetComponent<BoxCollider>().enabled = false;
        transform.tag = "Cursor";
        setColor("blue");
        gameObject.layer = 2; // set for cursorCube material, tag, color and layer
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setColor(string name) // set color for cursorCube depending on the situation
    {
        if (name == "red")
            GetComponent<Renderer>().material.color = tRed;
        else
        if (name == "blue")
            GetComponent<Renderer>().material.color = tBlue;
        else
        if (name == "green")
            GetComponent<Renderer>().material.color = tGreen;
    }

    void prepareToOperate(RaycastHit hit) // determination position hit of cube and ray
    {
        if (hit.transform.tag == "Buildable") // if ray collide with cube or other object
        {
            if (hit.normal.x > 0.999) 
            {
                inCastlePos = hit.transform.GetComponent<Builded>().getInCastlePos() + new Vector3(1, 0, 0);
            }
            else
            if (hit.normal.x < -0.999)
            {
                inCastlePos = hit.transform.GetComponent<Builded>().getInCastlePos() - new Vector3(1, 0, 0);
            }
            else
            if (hit.normal.y > 0.999)  //direction -> up
            {
                inCastlePos = hit.transform.GetComponent<Builded>().getInCastlePos() + new Vector3(0, 1, 0);
            }
            else
            if (hit.normal.y < -0.999) //direction -> down
            {
                inCastlePos = hit.transform.GetComponent<Builded>().getInCastlePos() - new Vector3(0, 1, 0);
            }
            else
            if (hit.normal.z > 0.999)
            {
                inCastlePos = hit.transform.GetComponent<Builded>().getInCastlePos() + new Vector3(0, 0, 1);
            }
            else
            if (hit.normal.z < -0.999)
            {
                inCastlePos = hit.transform.GetComponent<Builded>().getInCastlePos() - new Vector3(0, 0, 1);
            }
        }
        else if (hit.transform.tag == "Grownd") // if ray collide with ground
        {
            inCastlePos = castle.buildOnTheGrowndCoord(hit.point);
        }

            transform.position = castle.getPosByElement(inCastlePos);
    }

    void operate(int button) // make new cube in cubeCursor, which have new characteristic
    {
        if (button == 0)
        {
            GameObject tmp = Instantiate(gameObject, transform.position, transform.rotation);
            tmp.GetComponent<Cursor>().enabled = false;
            tmp.GetComponent<Builded>().enabled = true;
            tmp.GetComponent<Builded>().setInCastlePos(inCastlePos);
            castle.blocks.Add(tmp.GetComponent<Builded>());
        }
    }
}
