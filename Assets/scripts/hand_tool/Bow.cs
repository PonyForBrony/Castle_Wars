using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public GameObject arrow;
    public float rechargeDelay;

    private GameObject instance;
    private float startTime;

    private bool startAiming = false;
    private float currentAngle = 0, step = 10, maxAngle = 60;
    public Vector3 bowEndPosition = new Vector3(0, -0.3f, 1.0f); // position when bow/arrow in the middle - endPosition
    public Vector3 bowStartPosition = new Vector3(0.4f, -0.4f, 0.8f); // position when bow/arrow in the standart position - startPosition
    public Vector3 arrowEndPosition = new Vector3(0, 0, -0.3f);
    public Vector3 arrowStartPosition = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start()
    {
        instance = Instantiate(arrow, transform);

        transform.localPosition = bowStartPosition;
        instance.transform.localPosition = arrowStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (instance == null && Time.time - startTime >= rechargeDelay)
        {
            instance = Instantiate(arrow, transform);
        }

        if (startAiming)
        {
            if (currentAngle >= -maxAngle)
            {
                currentAngle = currentAngle - maxAngle / step;
                if (currentAngle >= -maxAngle)
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, currentAngle), 1);
                else
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, -maxAngle), 1);
            }
            if (transform.localPosition != bowEndPosition)
            {
                transform.localPosition += (bowEndPosition - bowStartPosition) / step;
                instance.transform.localPosition += (arrowEndPosition - arrowStartPosition) / step;
            }
        }

        if (!startAiming)
        {
            if (currentAngle <= 0)
            {
                currentAngle = currentAngle + maxAngle / step;
                if (currentAngle < 0)
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, currentAngle), 1);
                else
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), 1);
            }
            if (transform.localPosition != bowStartPosition)
            {
                transform.localPosition += (bowStartPosition - bowEndPosition) / step;
                instance.transform.localPosition += (arrowStartPosition - arrowEndPosition) / step;
            }
        }
    }

    void operate(int button)
    {
        if (button == 0 && Time.time - startTime >= rechargeDelay && instance != null)
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
            startAiming = true;
        }
        else if (button == 3)
        {
            //Debug.Log(instance.transform.localPosition);
            startAiming = false;
        }
    }
}
