using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class AxleInfo
{
    public WheelController leftWheel;
    public WheelController rightWheel;
    public bool motor;
    public bool steering;
}
public class VehicleController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;
    public float maxMotorTorqueForward;
    public float maxMotorTorqueReverse;
    public float maxBreakTorque;


    [Header("Outputs")]
    public float steerInput;
    public float motor;

    private float ackermannAngleLeft;
    private float ackermannAngleRight;

    private void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        steerInput = moveInput.x ;

        if (Vector3.Dot(gameObject.GetComponent<Rigidbody>().velocity, transform.forward) > 0)
        {
            if (moveInput.z > 0)
            {
                motor = maxMotorTorqueForward * moveInput.z;
            }
            else if (moveInput.z < 0)
            {
                motor = maxBreakTorque * moveInput.z;
            }
            else
            {
                motor = 0;
            }
        }
        else
        {
            if (moveInput.z > 0)
            {
                motor = maxMotorTorqueForward * moveInput.z;
            }
            else if (moveInput.z < 0)
            {
                motor = maxMotorTorqueReverse * moveInput.z;

            }
            else
            {
                motor = 0;
            }
        }

        if (steerInput > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        } 
        else if (steerInput < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius = (rearTrack / 2))) * steerInput;
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach(AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steeringAngle = ackermannAngleLeft;
                axleInfo.rightWheel.steeringAngle = ackermannAngleRight;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }
}
