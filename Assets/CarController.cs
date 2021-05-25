using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 45.0F;
    public float turnSpeed = 5.0F;

    public float maxMotorTorque = 80.0F;
    public float minMotorTorque = 50.0F;
    public float maxBrakeTorque = 150.0F;
    [ReadOnly]
    public float currentSpeed;
    public float maxSpeed = 100.0F;
    public float minSpeed = 50.0F;
    public Vector3 centerOfMass;
    [ReadOnly]
    public bool isBreaking;
    [ReadOnly]
    public bool slowDown;

    [Header("Wheels")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    [Header("Rendering")]
    public Texture2D textureNormal;
    public Texture2D textureBraking;
    public Renderer carRenderer;

    [Header("Sensors")]
    public float sensorLength = 3.0F;
    public Vector3 frontSensorPos = new Vector3(0, 0.2F, 0.93F);
    public float frontSideSensorPos = 0.1F;
    public float frontSensorAngle = 30.0F;

    private List<Transform> nodes;
    private int currentNode = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckWayPoint();
        Braking();
        LerpToSteerAngle();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        float avoidMultiplier = 0;
        avoiding = false;

        // Front right sensor
        sensorStartPos += transform.right * frontSideSensorPos;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") || !hit.collider.CompareTag("Point"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1F;
            }
        }

        // Front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") || !hit.collider.CompareTag("Point"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5F;
            }
        }

        // Front left sensor
        sensorStartPos -= transform.right * frontSideSensorPos * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") || !hit.collider.CompareTag("Point"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1F;
            }
        }

        // Front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") || !hit.collider.CompareTag("Point"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5F;
            }
        }

        // Front center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Road") || !hit.collider.CompareTag("Point"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }
            }
        }

        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }
    }

    private void ApplySteer()
    {
        if (avoiding) { return; }
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        //float speedTo = maxMotorTorque;
        //float currentVelocity = speedTo * 1.5F;
        //if (currentSpeed < maxSpeed && !isBreaking)
        //{
        //    wheelFL.motorTorque = maxMotorTorque;
        //    wheelFR.motorTorque = maxMotorTorque;
        //}
        //else
        //{
        //    wheelFL.motorTorque = 0;
        //    wheelFR.motorTorque = 0;
        //}
        if (nodes[currentNode].position.y < transform.position.y || slowDown) // Slows the car down if it is on a hill
        {
            if (currentSpeed > minSpeed && !isBreaking && currentSpeed <= 15)
            {
                wheelFL.motorTorque = minMotorTorque;
                wheelFR.motorTorque = minMotorTorque;
                wheelBL.motorTorque = maxMotorTorque;
                wheelBR.motorTorque = maxMotorTorque;
                //wheelFL.motorTorque = 8000f * Time.deltaTime * currentVelocity;
                //wheelFR.motorTorque = 8000f * Time.deltaTime * currentVelocity;
                //wheelBL.motorTorque = 8000f * Time.deltaTime * currentVelocity;
                //wheelBR.motorTorque = 8000f * Time.deltaTime * currentVelocity;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
                wheelBL.motorTorque = 0;
                wheelBR.motorTorque = 0;
            }
        }
        //{
        //    isBreaking = true;
        //}
        else
        {
            if (currentSpeed < maxSpeed && !isBreaking)
            {
                wheelFL.motorTorque = maxMotorTorque;
                wheelFR.motorTorque = maxMotorTorque;
                wheelBL.motorTorque = maxMotorTorque;
                wheelBR.motorTorque = maxMotorTorque;
                //wheelFL.motorTorque = 8000f * Time.deltaTime * currentVelocity;
                //wheelFR.motorTorque = 8000f * Time.deltaTime * currentVelocity;
                //wheelBL.motorTorque = 8000f * Time.deltaTime * currentVelocity;
                //wheelBR.motorTorque = 8000f * Time.deltaTime * currentVelocity;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
                wheelBL.motorTorque = 0;
                wheelBR.motorTorque = 0;
            }
        }

        if (currentNode != 0 && currentNode != nodes.Count -1)
        {
            if (Vector3.Distance(nodes[currentNode - 1].position, nodes[currentNode].position) < 5.0F)
            {
                slowDown = true;
            }
            else
            {
                slowDown = false;
            }
        }
    }

    private void CheckWayPoint()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 5.0F)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
        //Debug.Log(nodes[currentNode].gameObject.name);
        //Debug.Log(nodes.Count);
    }

    private void Braking()
    {
        if (isBreaking)
        {
            carRenderer.material.mainTexture = textureBraking;
            wheelBL.brakeTorque = maxBrakeTorque;
            wheelBR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            carRenderer.material.mainTexture = textureNormal;
            wheelBL.brakeTorque = 0;
            wheelBR.brakeTorque = 0;
        }
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("SlowPoint"))
        {
            isBreaking = true;
            Debug.Log("Hit slow point");
        }
        if (other.gameObject.name.Contains("SpeedPoint"))
        {
            isBreaking = false;
            Debug.Log("Hit speed point");
        }
    }
}
