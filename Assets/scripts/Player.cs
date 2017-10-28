using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject Cursor;

    public float maxBuildDist;

    private Ray rayToLand;  //ray from camera to operate whith objects
    private RaycastHit[] hits;

    public GameObject handTool, block;
    private GameObject operateObj;    //current player choice

    private int actionMode;    //1->build , 2->operate with handed , 3->operate with landed


    // Use this for initialization
    void Start()
    {
        rayToLand = new Ray();
        actionMode = 3;
    }

    // Update is called once per frame
    void Update()
    {

        rayToLand.origin = transform.position;
        rayToLand.direction = transform.forward;
        hits = Physics.RaycastAll(rayToLand, maxBuildDist).OrderBy(h => h.distance).ToArray();
        if (Input.anyKey || Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (keyListener() == 4) //state == change object
                changeObject(actionMode);
        }

        String objects = "";
        foreach (RaycastHit hit in hits)
            objects += hit.transform.name + "  ";
        Debug.Log(objects + "  " + hits.Length); //printing all objects on the Ray and length of array of them

        switch (actionMode)
        {
            case 1:
                if (hits.Length > 0)
                {
                    if (operateObj == null)
                        changeObject(actionMode);
                    Debug.DrawLine(transform.position, hits[0].point, Color.blue);
                    operateObj.SendMessage("prepareToOperate", hits[0]);
                    
                }
                else
                    Destroy(operateObj);
                    
                break;

                /*case 2:
                    operateObj.SendMessage("prepareToOperate", hits[0]);
                    break;                 // maybe will use...*/

                /*case 3:
                    hits[0].transform.SendMessage("prepareToOperate", hits[0]);
                    break;                 //will used when we create builded operateble objects */
        }
    }

    int keyListener()  //return binary code that has mouse button in 1 digit , changeMode in 2 , PhysPouse in 3
    {
        byte a = 0, b = 0, c = 0;
        if (operateObj != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                operateWithObj(0);
                a = 1;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                operateWithObj(1);
                a = 1;
            }
        }

        int tmp = actionMode;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            actionMode = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            actionMode = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            actionMode = 3;
        b = (byte)(tmp != actionMode? 1 : 0);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PhysPouse.on = !PhysPouse.on;
            c = 1;
        }
        return a * 2 + b * 4 + c * 8;
    }

    private void changeObject(int mode)
    {
        if (operateObj != null)
        Destroy(operateObj);
        switch (mode)
        {
            case 1:
                operateObj = Instantiate(block, hits[0].point, Quaternion.identity);

                break;

            case 2:
                operateObj = Instantiate(handTool, transform);
                operateObj.transform.localPosition = handTool.transform.position;
                operateObj.transform.localRotation = handTool.transform.rotation;
                break;

            case 3:
                break;
        }
    }

    private void operateWithObj(int button)
    {
        if (actionMode < 3)
            operateObj.SendMessage("operate", button);
        /*else
            hits[0].transform.SendMessage("operate"); //will used when we create builded operateble objects */
    }
}
