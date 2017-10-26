using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject buildBlock, castleHandler;
    private Castle castle;

    private GameObject localCursor;

    private bool getBlock, putBlock, onSurface;

    private float distance;

    private Vector3 blockSizeX, blockSizeY, blockSizeZ;
    private Vector3 blockPos;

    private Ray rayToLand;
    private RaycastHit[] hits;

    // Use this for initialization
    void Start()
    {
        onSurface = false;
        castle = castleHandler.GetComponent<Castle>();
        getBlock = true;
        putBlock = false;
        distance = 4;
        blockSizeX = new Vector3((buildBlock.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * buildBlock.transform.lossyScale.x) / 2, 0, 0);
        blockSizeY = new Vector3(0, (buildBlock.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * buildBlock.transform.lossyScale.x) / 2, 0);
        blockSizeZ = new Vector3(0, 0, (buildBlock.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * buildBlock.transform.lossyScale.x) / 2);

        rayToLand = new Ray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey || Input.GetAxis("Mouse ScrollWheel") != 0f)
            keyListener();

        rayToLand.origin = transform.position;
        rayToLand.direction = transform.forward;
        hits = Physics.RaycastAll(rayToLand, distance).OrderBy(h => h.distance).ToArray();

        if (getBlock && hits.Length != 0)
        {
            if (localCursor == null)
            {
                localCursor = Instantiate(buildBlock);
                localCursor.tag = "Cursor";
                localCursor.SendMessage("onCursor");
                localCursor.SendMessage("setColor", "blue");
            }

            foreach (RaycastHit hit in hits)
                if (hit.transform.tag == "Buildable")
                {
                    onSurface = true;
                    if (hit.transform.position.y < 0)
                        localCursor.GetComponent<CursorCube>().inCastlePos = castle.buildOnTheGrowndCoord(hit.point);
                    else
                    if (hit.normal.x > 0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos + new Vector3(1, 0, 0);
                        //Debug.Log("1");
                    }
                    else
                    if (hit.normal.x < -0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos - new Vector3(1, 0, 0);
                        //Debug.Log("2");
                    }
                    else
                    if (hit.normal.y > 0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos + new Vector3(0, 1, 0);
                        //Debug.Log("3");
                    }
                    else
                    if (hit.normal.y < -0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos - new Vector3(0, 1, 0);
                        //Debug.Log("4");
                    }
                    else
                    if (hit.normal.z > 0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos + new Vector3(0, 0, 1);
                        //Debug.Log("5");
                    }
                    else
                    if (hit.normal.z < -0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos - new Vector3(0, 0, 1);
                        //Debug.Log("6");
                    }
                    //Debug.Log(hit.normal.ToString());


                    localCursor.transform.position = castle.getPosByElement(localCursor.GetComponent<CursorCube>().inCastlePos);
                    //Debug.Log(localCursor.transform.position);
                    Debug.DrawLine(transform.position, hit.point, Color.blue);

                    if (putBlock)
                        doPutBlock();

                    break;
                }
                else onSurface = false;
        }
        else if (localCursor != null)
        {
            onSurface = false;
            Destroy(localCursor);
        }
    }

    private void doPutBlock()
    {
        putBlock = false;
        GameObject tmp = Instantiate(buildBlock, localCursor.transform.position, Quaternion.identity);
        tmp.SendMessage("onInstantiate");
        tmp.GetComponent<CursorCube>().inCastlePos = localCursor.GetComponent<CursorCube>().inCastlePos;
        tmp.tag = "Buildable";
    }

    void keyListener()
    {
        if (Input.GetKeyDown(KeyCode.O) || Input.GetMouseButtonDown(1))
            if (getBlock)
                getBlock = false;
            else
                getBlock = true;


        if ((Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)) && onSurface == true)
            putBlock = true;

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            distance = Mathf.Clamp(distance + Input.GetAxis("Mouse ScrollWheel") * 0.5f, 1, 4);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PhysPouse.on = !PhysPouse.on;
        }
    }
}
