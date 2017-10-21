using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlocksList {
    public static List<Block> blocks = new List<Block>();
}


public struct Block
{
    public GameObject block;
    public bool isAvalible;
}
