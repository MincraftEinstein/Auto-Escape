using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController3 : MonoBehaviour
{
    [SerializeField] int wheelsOnGround;
    //[SerializeField] private float horsePower = 1000.0F;
    //[SerializeField] float turnSpeed = 45.0F; // Speed the vehicle turns
    //[SerializeField] float speed;
    //[SerializeField] float rpm;
    [SerializeField] Vector3 centerOfMass;
    ////[SerializeField] TextMeshProUGUI speedometerText;
    ////[SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] List<WheelCollider> allWheels;
    private float horizontalInput;
    private float forwardInput;
    private Rigidbody RB;

    public float maxSteerAngle = 45.0F;
    public float turnSpeed = 5.0F;

    public float maxMotorTorque = 80.0F;
    public float currentSpeed;
    public float maxSpeed = 100.0F;
    private float targetSteerAngle = 0;

    [Header("Wheels")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.centerOfMass = centerOfMass;//.transform.position;
        allWheels.Add(wheelFL);
        allWheels.Add(wheelFR);
        allWheels.Add(wheelBL);
        allWheels.Add(wheelBR);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Gets the player input
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        Drive();
        ApplySteer();
        LerpToSteerAngle();

        //if (IsOnGround())
        //{
        //    // Moves the vehicle forward
        //    //transform.Translate(Vector3.forward * Time.deltaTime * vehicleSpeed * forwardInput);
        //    RB.AddRelativeForce(Vector3.forward * forwardInput * horsePower);

        //    // Turns/rotates the vehicle
        //    transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        //    speed = Mathf.Round(RB.velocity.magnitude * 2.237F);
        //    //speedometerText.SetText("MPH: " + speed);

        //    rpm = Mathf.Round((speed % 30) * 40);
        //    //rpmText.SetText("RPM: " + rpm);
        //}
    }

    void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && IsOnGround())
        {
            wheelFL.motorTorque = maxMotorTorque * forwardInput;
            wheelFR.motorTorque = maxMotorTorque * forwardInput;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void ApplySteer()
    {
        //Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        //relativeVector /= relativeVector.magnitude;
        float newSteer = /*(relativeVector.x / relativeVector.magnitude)*/ horizontalInput * maxSteerAngle;
        targetSteerAngle = newSteer;
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }
        if (wheelsOnGround == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
