using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject buildBlock;
    public Transform BlockCoordSys;
    public float cellSize = 0.1f;

    private GameObject localCursor;

    private bool getBlock, putBlock, onSurface;
    private int canBuild;

    private float distance;

    private Vector3 blockSizeX, blockSizeY, blockSizeZ;
    private Vector3 blockPos;

    private Ray rayToLand;
    private RaycastHit[] hits;


    // Use this for initialization
    void Start()
    {
        BlockCoordSys = Instantiate(BlockCoordSys, transform.position, Quaternion.identity);
        getBlock = false;
        putBlock = false;
        canBuild = 0;
        onSurface = false;
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

        if (getBlock)
        {
            if (localCursor == null)
            {
                localCursor = Instantiate(buildBlock);
                localCursor.tag = "Cursor";
                localCursor.SendMessage("onCursor");
                localCursor.GetComponent<MeshCollider>().enabled = false;
                localCursor.GetComponent<Collider>().isTrigger=true;
                localCursor.GetComponent<BoxCollider>().size = new Vector3(0.999f,0.999f,0.999f);
                localCursor.transform.SetParent(BlockCoordSys);
            }

            rayToLand.origin = transform.position;
            rayToLand.direction = transform.forward;

            onSurface = false;
            canBuild = 0;
            hits = Physics.RaycastAll(rayToLand, distance).OrderBy(h => h.distance).ToArray();
            if (hits.Length != 0)
                foreach (RaycastHit hit in hits)
                    if (hit.transform.tag == "Buildable")
                    {
                        onSurface = true;

                        if (Math.Abs(hit.normal.x)==1 || Math.Abs(hit.normal.y) == 1 || Math.Abs(hit.normal.z) == 1)
                        {
                            canBuild = 1;
                            BlockCoordSys.position = hit.point;
                            BlockCoordSys.rotation = Quaternion.FromToRotation(BlockCoordSys.up, hit.normal) * BlockCoordSys.rotation;
                            blockPos = new Vector3(hit.point.x - BlockCoordSys.position.x, blockSizeY.y, hit.point.z - BlockCoordSys.position.z);
                        }
                        else
                        {
                            canBuild = 2;
                            BlockCoordSys.position = hit.point;
                            BlockCoordSys.rotation = Quaternion.FromToRotation(BlockCoordSys.up, hit.normal) * BlockCoordSys.rotation;
                            blockPos = new Vector3(hit.point.x - BlockCoordSys.position.x, blockSizeY.y, hit.point.z - BlockCoordSys.position.z);
                        }
                        Debug.DrawLine(transform.position, hit.point);
                        canBuildHere();
                        break;
                    }

            if (!onSurface)
            {
                BlockCoordSys.position = transform.position;
                BlockCoordSys.rotation = Quaternion.identity;
                blockPos = transform.forward * distance + blockSizeY;
            }

            switch (canBuild)
            {
                case 0:
                    localCursor.SendMessage("setColor", "red");
                    break;

                case 1:
                    localCursor.SendMessage("setColor", "blue");
                    break;

                case 2:
                    localCursor.SendMessage("setColor", "green");
                    break;
            }
            //Debug.Log(blockPos);

            localCursor.transform.localPosition = blockPos;

            if (putBlock && canBuild !=0)
                doPutBlock();
        }
        else if (localCursor != null)
            Destroy(localCursor);
    }

    private void canBuildHere()
    {
        bool tmp = true;
        tmp = localCursor.GetComponent<CursorCube>().isColliding();
        if (tmp)
        {
            canBuild = 0;
            localCursor.SendMessage("setColor", "red");
        }
    }

    /*private Vector3 cursorOnSurface(Vector3 objPos)
    {
        canBuild = true;
        objPos.Set((int)(objPos.x * ceilSize) / ceilSize, objPos.y, (int)(objPos.z * ceilSize) / ceilSize);
        //Debug.Log(objPos.x + "    " + objPos.z);
        localCursor.SendMessage("setColor", "blue");
        return objPos;
    }*/

    private void doPutBlock()
    {
        putBlock = false;
        GameObject tmp = Instantiate(buildBlock, localCursor.transform.position, BlockCoordSys.rotation);
        tmp.SendMessage("onInstantiate");
        //tmp.GetComponent<MeshCollider>().enabled = true;
        tmp.tag = "Buildable";
    }

    void keyListener()
    {
        if (Input.GetKeyDown(KeyCode.O) || Input.GetMouseButtonDown(1))
            if (getBlock)
                getBlock = false;
            else
                getBlock = true;


        if ((Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)) && canBuild !=0)
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
