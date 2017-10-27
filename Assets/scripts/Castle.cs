using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public GameObject[,,] Blocks;
    public float cellSize;
    public int sizeX, sizeY, sizeZ;

    public void Start()
    {
        //Blocks = new GameObject[sizeX,sizeY,sizeZ];
    }

    public Vector3 buildOnTheGrowndCoord(Vector3 pos)
    {
        Debug.Log(pos.y + cellSize / 2);
        return new Vector3(Mathf.RoundToInt((pos.x - transform.position.x) / cellSize), 0, Mathf.RoundToInt((pos.z - transform.position.z) / cellSize));
    }

    public Vector3 getPosByElement(Vector3 element)
    {
        return transform.position + element * cellSize + new Vector3(0,cellSize/2,0);
    }

}
