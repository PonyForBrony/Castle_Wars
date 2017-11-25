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
    private bool doAim = false, aimMode = false;
    private float angle = 0, step = 10, maxAngle = 60, angleTmp;
    public Vector3 aimPosition = new Vector3(0, -0.4f, 0.75f);
    public Vector3 startPosition = new Vector3(0.4f, -0.4f, 0.8f);
    public Vector3 arrowPosition = new Vector3(0, 0, 0.7f);
    public Vector3 arrowStartPosition = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start()
    {
        transform.localPosition = startPosition;
        instance = Instantiate(arrow, transform);
        instance.transform.localPosition = arrowStartPosition;
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
            if (aimMode)
            {
                if (angle >= -maxAngle)
                {
                    angle = angle - maxAngle / step;
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, angle - maxAngle / step), 1);
                    angleTmp = -angle;
                }
                if (transform.localPosition != aimPosition)
                {
                    transform.localPosition += (aimPosition - startPosition) / step;
                    instance.transform.localPosition += (arrowPosition - arrowStartPosition) / step;
                }
            }
            
            if (!aimMode)
            {
                if (angle <= 0)
                {
                    angle = angle + angleTmp / step;
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, angle), 1);
                }
                if (transform.localPosition != startPosition)
                {
                    transform.localPosition += (startPosition - aimPosition) / step;
                    instance.transform.localPosition += (arrowStartPosition - arrowPosition) / step;
                }
            }
        }
    }

    void operate(int button)
    {
        if (button == 0 && Time.time-startTime >= rechargeDelay && instance != null)
        {
            instance.transform.parent = null;
            instance.transform.tag = "Fallen";
            instance.GetComponent<Arrow>().enabled = true;
            instance = null;
            startTime = Time.time;
        }
        else if (button == 1)
        {
            //Debug.Log(instance.transform.localPosition);
            doAim = true;
            aimMode = true;
        }
        else if (button == 3)
        {
            //Debug.Log(instance.transform.localPosition);
            aimMode = false;
        }
    }
}
