using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public List<Builded> blocks;
    public float cellSize;

    public void Start()
    {
        blocks = new List<Builded>();
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

}