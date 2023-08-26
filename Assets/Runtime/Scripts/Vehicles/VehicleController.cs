using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public WheelController[] wheels;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;

    [Header("Inputs")]
    public float steerInput;

    private float ackermannAngleLeft;
    private float ackermannAngleRight;

    private void Update()
    {
        steerInput = Input.GetAxis("Horizontal");
        
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

        foreach(WheelController w in wheels)
        {
            if (w.wheelPosition == WheelPosition.FrontLeft)
            {
                w.steeringAngle = ackermannAngleLeft;
            } 
            else if (w.wheelPosition == WheelPosition.FrontRight)
            {
                w.steeringAngle = ackermannAngleRight;
            }
        }
    }
}
