using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITreeTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        ITree<int> key = new ITree<int>();
        ITreeElement<int> current = key.getRoot();
        current.addChild(1).addChild(2).addChild(3).addChild(4).setBranchName("branch 1");
        current = current.addChild(100);
        for (int i = 1; i <= 10; i++)
        {
            current.addChild(i * 2).setBranchName("iterative branch " + i);
        }
        current = current.getParent();
        for (int i = 1; i <= 10; i++)
        {
            current = current.addChild(i * 21);
        }
        current.setBranchName("branch 3");

        foreach (ITreeBranch<int> b in key.getBranches(key.getRoot()))
        {
            Debug.Log(b.ToString());
        }
    }

}
