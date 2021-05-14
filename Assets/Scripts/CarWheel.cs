using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public WheelCollider wheel;

    private Vector3 wheelPos = new Vector3();
    private Quaternion wheelRotation = new Quaternion();

    // Update is called once per frame
    void Update()
    {
        wheel.GetWorldPose(out wheelPos, out wheelRotation);
        transform.position = wheelPos;
        transform.rotation = wheelRotation;
    }
}
