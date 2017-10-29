using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public GameObject arrow;
    public float rechargeDelay;

    private GameObject instance;
    private float startTime;

    // Use this for initialization
    void Start()
    {
        instance = Instantiate(arrow, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (instance == null && Time.time - startTime >= rechargeDelay)
        {
            instance = Instantiate(arrow, transform);
        }
    }

    void operate(int button)
    {
        if (button == 0 && Time.time-startTime >= rechargeDelay)
        {
            instance.transform.parent = null;
            instance.GetComponent<Arrow>().enabled = true;
            instance = null;
            startTime = Time.time;
        }
    }
}
