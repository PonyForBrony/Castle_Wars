using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public GameObject arrow;
    public float rechargeDelay;

    private GameObject instance;
    private float startTime;

    private Quaternion startAngle;
    bool doAim = false;
    bool aimMode = false;
    private float angle = 0;
    private float maxAngle = 80f;
    public float step = 3;

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

        if (doAim)
        {
            if (angle >= -maxAngle && aimMode)
            {
                angle -= step;
                transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), 1);
            }
            
            if (!aimMode && angle <= 0)
            {
                angle += step;
                transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), 1);
            }
        }
    }

    void operate(int button)
    {
        if (button == 0 && Time.time-startTime >= rechargeDelay && instance != null && angle < -75)
        {
            instance.transform.parent = null;
            instance.transform.tag = "Fallen";
            instance.GetComponent<Arrow>().enabled = true;
            instance = null;
            startTime = Time.time;
        }
        else if (button == 1)
        {
            doAim = true;
            aimMode = true;
        }
        else if (button == 3)
        {
            aimMode = false;
        }
    }
}
