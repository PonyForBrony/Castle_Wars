using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public List<Builded> castleBlocks;
    public float cellSize;

    public void Start()
    {
        castleBlocks = new List<Builded>();
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

    private List<Builded>[] clouds;
    private int[] isBranchEnded; // = 0 if ended ; = -1 if on the ground

    void checkForBuildClouds(Vector3 inCastleBlockPos)
    {
        checkForBuildClouds(findByCastlePos(inCastleBlockPos), true, 0);
    }

    void checkForBuildClouds(Builded block, bool firstIteration, int branch)  // recoursive blocks-clouds detecting
    {
        if (firstIteration)
        {
            isBranchEnded = new int[6];  //block has 6 sides => we have 6 variants of branches
            clouds = new List<Builded>[6];

            for (int i = 0; i < 6; i++)
                clouds[i] = new List<Builded>();

                for (int i = 0; i < 6; i++)
                isBranchEnded[i] = 0;

            foreach (Builded item in castleBlocks)
                item.isChekedForCloud = false;
        }
        else
        {
            if (isBranchEnded[branch] < 0)
                return;

            block.isChekedForCloud = true;

            clouds[branch].Add(block);
            if (block.getInCastlePos().y == 0)
                isBranchEnded[branch] = -1;
        }

        Builded tmp = findByCastlePos(block.getInCastlePos() + Vector3.up);
        if (block.canBuildOnTop && tmp != null && tmp.canBuildOnBottom && !tmp.isChekedForCloud)
        {
            if (!firstIteration)
            {
                isBranchEnded[branch]++;
                checkForBuildClouds(tmp, false, branch);
            }
            else
                checkForBuildClouds(tmp, false, 0);
            tmp = null;
        }

        tmp = findByCastlePos(block.getInCastlePos() + Vector3.down);
        if (block.canBuildOnBottom && tmp != null && tmp.canBuildOnTop && !tmp.isChekedForCloud)
        {
            if (!firstIteration)
            {
                isBranchEnded[branch]++;
                checkForBuildClouds(tmp, false, branch);
            }
            else
                checkForBuildClouds(tmp, false, 1);
            tmp = null;
        }

        tmp = findByCastlePos(block.getInCastlePos() + Vector3.forward);
        if (block.canBuildOnFront && tmp != null && tmp.canBuildOnBack && !tmp.isChekedForCloud)
        {
            if (!firstIteration)
            {
                isBranchEnded[branch]++;
                checkForBuildClouds(tmp, false, branch);
            }
            else
                checkForBuildClouds(tmp, false, 2);
            tmp = null;
        }

        tmp = findByCastlePos(block.getInCastlePos() + Vector3.back);
        if (block.canBuildOnBack && tmp != null && tmp.canBuildOnFront && !tmp.isChekedForCloud)
        {
            if (!firstIteration)
            {
                isBranchEnded[branch]++;
                checkForBuildClouds(tmp, false, branch);
            }
            else
                checkForBuildClouds(tmp, false, 3);
            tmp = null;
        }

        tmp = findByCastlePos(block.getInCastlePos() + Vector3.left);
        if (block.canBuildOnLeft && tmp != null && tmp.canBuildOnRight && !tmp.isChekedForCloud)
        {
            if (!firstIteration)
            {
                isBranchEnded[branch]++;
                checkForBuildClouds(tmp, false, branch);
            }
            else
                checkForBuildClouds(tmp, false, 4);
            tmp = null;
        }

        tmp = findByCastlePos(block.getInCastlePos() + Vector3.right);
        if (block.canBuildOnRight && tmp != null && tmp.canBuildOnLeft && !tmp.isChekedForCloud)
        {
            if (!firstIteration)
            {
                isBranchEnded[branch]++;
                checkForBuildClouds(tmp, false, branch);
            }
            else
                checkForBuildClouds(tmp, false, 5);
            tmp = null;
        }

        if (!firstIteration)
        {
            isBranchEnded[branch]--;
            if (isBranchEnded[branch] == 0)
                letThisCloudDown(clouds[branch]);
        }
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

}