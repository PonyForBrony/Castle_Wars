using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builded : MonoBehaviour
{
    public Material cubeMaterial;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material = cubeMaterial;
        gameObject.layer = 0;
        transform.tag = "Buildable";
    }

}
