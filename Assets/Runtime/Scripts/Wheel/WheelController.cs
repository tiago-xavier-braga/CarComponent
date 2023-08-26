using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    [HideInInspector]
    public float motorTorque;
    [HideInInspector]
    public float breakTorque;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;
    private float forceX;
    private float forceY;

    private Vector3 suspensionForce;
    private Vector3 wheelVelocityLocalSpace;

    [Header("Wheel")]
    public float wheelRadius;
    public float steeringAngle;
    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.x, 
            transform.localRotation.y + steeringAngle, 
            transform.localRotation.z);

        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.green);
    }

    private void FixedUpdate()
    {
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            wheelVelocityLocalSpace = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            forceX = motorTorque;
            forceY = wheelVelocityLocalSpace.x * springForce;

            rb.AddForceAtPosition(suspensionForce + (forceX * transform.forward) + (forceY * -transform.right), hit.point);
        }
    }
}
