using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builded : MonoBehaviour
{
    public Material cubeMaterial;

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material = cubeMaterial;
        gameObject.layer = 0;
        transform.tag = "Buildable";
    }

    // Update is called once per frame
    void Update()
    {
    }

}
