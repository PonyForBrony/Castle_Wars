using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public float HP, destroyTime;
    private bool isDestroyed;
    private Vector3 size0, delta;

    // Use this for initialization
    void Start()
    {
        isDestroyed = false;
        size0 = transform.localScale;
        delta = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            if (GetComponent<Builded>() != null)
            {
                delta.Set((size0.x / destroyTime) * Time.deltaTime, (size0.y / destroyTime) * Time.deltaTime, (size0.z / destroyTime) * Time.deltaTime);
                if (transform.localScale.magnitude > delta.magnitude)
                    transform.localScale -= delta;
                else
                {
                    transform.localScale.Set(0, 0, 0);
                    SendMessage("notAPIOnDestroy");
                    Destroy(gameObject);
                }
            }
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
