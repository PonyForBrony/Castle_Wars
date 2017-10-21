using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCube : MonoBehaviour
{

    public Color tRed, tBlue, tGreen;
    public Material instaceMaterial,cursorMaterial;
    private int collisions;

    // Use this for initialization
    void Start()
    {
        collisions = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setColor(string name)
    {
        if (name == "red")
            GetComponent<Renderer>().material.color = tRed;
        else
        if (name == "blue")
            GetComponent<Renderer>().material.color = tBlue;
        else
        if (name == "green")
            GetComponent<Renderer>().material.color = tGreen;
    }

    void onInstantiate()
    {
        GetComponent<Renderer>().material = instaceMaterial;
        GetComponent<CursorCube>().enabled = false;
    }

    void onCursor()
    {
        GetComponent<Renderer>().material = cursorMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        collisions++;
        //Debug.Log("enter");
    }

    private void OnCollisionStay(Collision collision)
    {
        /*Debug.Log(collision.contacts.Length);
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            Debug.DrawRay(contact.point, contact.normal*10, Color.white);
        }*/
    }

    void OnTriggerExit(Collider other)
    {
        collisions--;
        //Debug.Log("exit");
    }

    public bool isColliding()
    {
        //Debug.Log(collisions);
        return (collisions != 0);
    }
}
