using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public List<Builded> castleBlocks;
    public float cellSize;

    void Awake()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
            createChildrenHandler(obj);
    }

    public void Start()
    {
        castleBlocks = new List<Builded>();
    }

    public void createChildrenHandler(GameObject obj)
    {
        //Debug.Log(obj.transform.localScale.ToString() + "   " + obj.name + "   " + obj.transform.tag);
        if ((obj.transform.tag == "Ground" || obj.transform.tag == "Buildable") && obj.transform.localScale != new Vector3(1, 1, 1))
        {
            //Debug.Log(obj.name);
            GameObject childrenHandler = new GameObject();
            childrenHandler.name = "ChildrenHandler";
            childrenHandler.transform.position = obj.transform.position;
            childrenHandler.transform.SetParent(obj.transform);
        }
    }

    public Vector3 buildOnTheGrowndCoord(Vector3 pos)
    {
        return new Vector3(Mathf.RoundToInt((pos.x - transform.position.x) / cellSize), 0, Mathf.RoundToInt((pos.z - transform.position.z) / cellSize));
    }

    public Vector3 getPosByElement(Vector3 element)
    {
        return transform.position + element * cellSize + new Vector3(0, cellSize / 2, 0);
    }

    private void saveToFile()
    {
        Debug.Log("Castle saved!");
    }

    private void loadFromFile()
    {
        Debug.Log("Castle loaded!");
    }

    private void Update()
    {
    }

    void checkForBuildClouds(Builded block)
    {
        Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
        List<Builded> cloud; //saving 6 clouds around destroyed cube to this iterated
        Builded tmp;

        castleBlocks.Remove(block);  //removing destroed block

        for (int i = 0; i < 6; i++)
        {
            tmp = findByCastlePos(block.getInCastlePos() + directions[i]);
            if (block.canBuildOn[i] && tmp != null && tmp.canBuildOn[block.getOppositeSide(i)])  //check tmp is exist and this block and tmp are coupling
            {
                cloud = new List<Builded>();
                if (!isBranchOnTheGrownd(findByCastlePos(block.getInCastlePos() + directions[i]), cloud, directions))
                    letThisCloudDown(cloud);
            }
        }
    }

    bool isBranchOnTheGrownd(Builded block, List<Builded> branch, Vector3[] directions)  // recoursive blocks-clouds detecting
    {
        bool isOnTheGrownd = false;
        Builded tmp;

        //block.GetComponent<Renderer>().material.color = Color.black; //function was-here visualization
        branch.Add(block);

        if (block.getInCastlePos().y == 0)
            return true;  //this block is on the ground 
        else
            for (int i = 0; i < 6; i++)
            {
                tmp = findByCastlePos(block.getInCastlePos() + directions[i]);
                if (block.canBuildOn[i] && tmp != null && tmp.canBuildOn[block.getOppositeSide(i)] && findByCastlePos(tmp.getInCastlePos(), branch) == null)  //check tmp is exist, this block and tmp are coupling, and tmp was not checked yet
                {
                    isOnTheGrownd = (isBranchOnTheGrownd(tmp, branch, directions) || isOnTheGrownd); //block is on the ground if one of his branches toching the ground
                    tmp = null;
                }
            }
        return isOnTheGrownd;
    }

    private void letThisCloudDown(List<Builded> cloud)
    {
        foreach (Builded block in cloud)
        {
            block.SendMessage("setFallen");
            castleBlocks.Remove(block);
        }
    }

    Builded findByCastlePos(Vector3 inCastlePos)
    {
        foreach (Builded block in castleBlocks)
        {
            if ((block.getInCastlePos() - inCastlePos).magnitude < 0.01)
            {
                return block;
            }
        }
        return null;
    }

    Builded findByCastlePos(Vector3 inCastlePos, List<Builded> list)
    {
        foreach (Builded block in list)
        {
            if ((block.getInCastlePos() - inCastlePos).magnitude < 0.01)
            {
                return block;
            }
        }
        return null;
    }

}