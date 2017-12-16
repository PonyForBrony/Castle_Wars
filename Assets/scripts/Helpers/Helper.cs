using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{

    public static bool ListEquals(List<KeyCode> a, List<KeyCode> b)//mast to be List<T> in future
    {
        if (a != null && b != null)
        {
            if (a.Count != b.Count)
                return false;
            else
                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i] != b[i])
                        return false;
                }

            return true;
        }
        else
            return true;
    }
}