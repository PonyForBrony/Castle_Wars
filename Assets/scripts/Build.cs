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

    public float maxBuildDist;

    private Vector3 blockSizeX, blockSizeY, blockSizeZ;
    private Vector3 blockPos;

    private Ray rayToLand;
    private RaycastHit[] hits;

    private GameObject operateObj;

    // Use this for initialization
    void Start()
    {
        onSurface = false;
        castle = castleHandler.GetComponent<Castle>();
        getBlock = true;
        putBlock = false;
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
        hits = Physics.RaycastAll(rayToLand, maxBuildDist).OrderBy(h => h.distance).ToArray();

        if (getBlock && hits.Length != 0)
        {

            /*String objects = "";
            foreach (RaycastHit hit in hits)
                objects += hit.transform.name + "  ";
            Debug.Log(objects + "  " + hits.Length); //printing all objects on the Ray*/

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
                    if (hit.normal.x > 0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos + new Vector3(1, 0, 0);
                    }
                    else
                    if (hit.normal.x < -0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos - new Vector3(1, 0, 0);
                    }
                    else
                    if (hit.normal.y > 0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos + new Vector3(0, 1, 0);
                    }
                    else
                    if (hit.normal.y < -0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos - new Vector3(0, 1, 0);
                    }
                    else
                    if (hit.normal.z > 0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos + new Vector3(0, 0, 1);
                    }
                    else
                    if (hit.normal.z < -0.999)
                    {
                        localCursor.GetComponent<CursorCube>().inCastlePos = hit.transform.gameObject.GetComponent<CursorCube>().inCastlePos - new Vector3(0, 0, 1);
                    }
                    Debug.DrawLine(transform.position, hit.point, Color.blue);
                    break;
                }
                else if (hit.transform.tag == "Grownd")
                {
                    onSurface = true;
                    localCursor.GetComponent<CursorCube>().inCastlePos = castle.buildOnTheGrowndCoord(hit.point);
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
                else onSurface = false;

            if (onSurface)
            {
                localCursor.transform.position = castle.getPosByElement(localCursor.GetComponent<CursorCube>().inCastlePos);
                if (putBlock)
                    doPutBlock();
            }
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
            maxBuildDist = Mathf.Clamp(maxBuildDist + Input.GetAxis("Mouse ScrollWheel") * 0.5f, 1, 4);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PhysPouse.on = !PhysPouse.on;
        }
    }
}
