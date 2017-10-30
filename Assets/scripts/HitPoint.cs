using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public float HP;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void applyDamage (float damage)
    {
        HP = HP - damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
