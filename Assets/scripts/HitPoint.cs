using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public float HP, destroyTime;
    private bool isDestroyed;
    private Vector3 size0;

    // Use this for initialization
    void Start()
    {
        isDestroyed = false;
        size0 = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            //Debug.Log("Death" + transform.localScale.ToString());
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(transform.localScale.x - (size0.x / destroyTime) * Time.deltaTime,
                                         transform.localScale.y - (size0.y / destroyTime) * Time.deltaTime,
                                         transform.localScale.z - (size0.z / destroyTime) * Time.deltaTime);
            else
                Destroy(gameObject);
        }
    }

    void applyDamage(float damage)
    {
        HP = HP - damage;
        if (HP <= 0)
            isDestroyed = true;
    }

    /*void SetGlobalScale(Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }*/

}
