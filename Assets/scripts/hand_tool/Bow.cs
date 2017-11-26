using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public GameObject arrow;
    public float rechargeDelay, aimingDelay;

    private GameObject instArrow;
    private float shootTime, aimingTime;

    private float angleDiff, positionDiff;

    private bool aiming = false;

    [Serializable]
    public class TransformState
    {
        [SerializeField]
        public Vector3 bowPosition, arrowPosition;

        [SerializeField]
        public Quaternion rotatition;

        public TransformState(Vector3 bpos, Vector3 rot, Vector3 apos)
        {
            bowPosition = bpos;
            rotatition = Quaternion.Euler(rot);
            arrowPosition = apos;
        }
    }

    public TransformState[] animStates = {new TransformState(new Vector3(0.4f, -0.4f, 0.8f), Vector3.zero, Vector3.zero),                   //default transform
                                          new TransformState(new Vector3(0, -0.3f, 1.0f), new Vector3(0,0,-60), new Vector3(0, 0, -0.3f))}; //aimed transform

    // Use this for initialization
    void Start()
    {
        instArrow = Instantiate(arrow, transform);

        transform.localPosition = animStates[0].bowPosition;
        instArrow.transform.localPosition = animStates[0].arrowPosition;

        angleDiff = Quaternion.Angle(animStates[0].rotatition, animStates[1].rotatition);
        positionDiff = (animStates[1].bowPosition - animStates[0].bowPosition).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (instArrow == null && Time.time - shootTime >= rechargeDelay)
        {
            instArrow = Instantiate(arrow, transform);
        }

        if (aiming)
        {
            if (aimingTime + aimingDelay > Time.time && transform.localRotation != animStates[1].rotatition)
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, animStates[1].rotatition, (Time.deltaTime * angleDiff) / aimingDelay);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, animStates[1].bowPosition, (Time.deltaTime * positionDiff) / aimingDelay);
                if (instArrow)
                    instArrow.transform.localPosition = Vector3.MoveTowards(instArrow.transform.localPosition, animStates[1].arrowPosition, (Time.deltaTime * positionDiff) / aimingDelay);
            }
        }
        else
        {
            if (aimingTime + aimingDelay > Time.time && transform.localRotation != animStates[0].rotatition)
            {

                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, animStates[0].rotatition, (Time.deltaTime * angleDiff) / aimingDelay);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, animStates[0].bowPosition, (Time.deltaTime * positionDiff) / aimingDelay);
                if (instArrow)
                    instArrow.transform.localPosition = Vector3.MoveTowards(instArrow.transform.localPosition, animStates[0].arrowPosition, (Time.deltaTime * positionDiff) / aimingDelay);
            }
        }
    }

    void operate(int button)
    {
        if (button == 0 && Time.time - shootTime >= rechargeDelay && instArrow != null)
        {
            instArrow.transform.parent = null;
            instArrow.transform.tag = "Fallen";
            instArrow.GetComponent<Arrow>().enabled = true;
            instArrow = null;
            shootTime = Time.time;
        }

        if (button == 1)
        {
            aimingTime = Time.time;
            //Debug.Log(instArrow.transform.localPosition);
            aiming = true;
        }
        else if (button == 3)
        {
            aimingTime = Time.time;
            //Debug.Log(instArrow.transform.localPosition);
            aiming = false;
        }
    }
}
