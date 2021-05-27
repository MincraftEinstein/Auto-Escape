using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CameraMode
{
    Default,
    Watch,
    Follow
}

/*!!READ ME!!
 * For the vehicle controller to properly function, you will need the following:
 * -A gameobject named "Chassis" as a child of the vehicle gameobject
 * -A gameobject named "Vehicle" with a set BoxCollider
 * -Child gameobjects for wheel models and wheel colliders (named Wheel_Models and Wheel_Colliders) with child gameobjects named to their respective positions
 * -A center of mass as child of the vehicle gameobject
 */

public class PlayerController : MonoBehaviour
{
    public bool canDrive = true;
    public TextMeshProUGUI speedText;

    //Camera
    public GameObject cam;
    private float cameraTilt = 12.0f;
    private float tiltSpeed = 0.1f;

    //uiCanvas
    private GameObject uiCanvas;

    //Vehicle
    public GameObject vehicleChassis;
    private GameObject wheelModels_GameObject;
    private GameObject wheelColliders_GameObject;

    public float vehicleSpeed = 5.0f;
    [ReadOnly]
    public float curVehicleSpeed = 0.0f;

    private float steering = 0;
    private float maxSteering = 30.0f;

    private float horizontalInput = 0;
    private float verticalInput = 0;

    public WheelCollider[] wheelColliders;
    public List<WheelCollider> frontWheels = new List<WheelCollider>();

    //Center of mass and Rigidbody
    private Rigidbody vehicleRb;
    public Vector3 centerOfMass;

    //Headlights
    private GameObject[] headlights;
    private bool headlightsActive;

    //Scripts
    private CameraControl cameraController;

    //Settings
    public CameraMode currentCameraMode;
    public CameraModes[] CameraModeData;

    public CameraMode GetEnums(CameraMode mode)
    {
        return mode;
    }

    public class CameraModes
    {
        public string name;
        public bool cameraControlEnabled;

        public CameraModes(string modeName, bool control)
        {
            name = modeName;
            cameraControlEnabled = control;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //cam = GameObject.Find("FocalPoint");

        uiCanvas = GameObject.Find("Canvas");

        //vehicleChassis = transform.Find("Chassis").gameObject;

        wheelModels_GameObject = transform.Find("Wheel_Models").gameObject;
        wheelColliders_GameObject = transform.Find("Wheel_Colliders").gameObject;
        Debug.Log(transform.Find("Wheel_Models").gameObject.name);
        Debug.Log(transform.Find("Wheel_Colliders").gameObject.name);

        //Rigidbody
        vehicleRb = transform.gameObject.AddComponent<Rigidbody>();
        vehicleRb.mass = 1200;
        vehicleRb.drag = 0; // 0.05f;
        vehicleRb.angularDrag = 0.03f;

        vehicleRb.centerOfMass = centerOfMass;

        //WheelColliders
        wheelColliders = new WheelCollider[wheelColliders_GameObject.transform.childCount];

        for (int i = 0; i < wheelColliders_GameObject.transform.childCount; i++)
        {
            GameObject wheel = wheelColliders_GameObject.transform.GetChild(i).gameObject;
            WheelCollider wheelCollider = wheel.AddComponent<WheelCollider>();
            wheelCollider.mass = 1;
            wheelCollider.radius = 0.25f;
            wheelCollider.wheelDampingRate = 0.5f;
            wheelCollider.suspensionDistance = 0.1f;

            var ss = wheelCollider.suspensionSpring;
            ss.spring = 4000;
            ss.damper = 2000;
            ss.targetPosition = 0.5f;

            var fwdFriction = wheelCollider.forwardFriction;
            fwdFriction.extremumSlip = 1;
            fwdFriction.extremumValue = 1;
            fwdFriction.asymptoteSlip = 1;
            fwdFriction.asymptoteValue = 1;
            fwdFriction.stiffness = 5;

            var sideFriction = wheelCollider.sidewaysFriction;
            sideFriction.extremumSlip = 1;
            sideFriction.extremumValue = 1;
            sideFriction.asymptoteSlip = 1;
            sideFriction.asymptoteValue = 1;
            sideFriction.stiffness = 5;

            wheelCollider.suspensionSpring = ss;
            wheelCollider.forwardFriction = fwdFriction;
            wheelCollider.sidewaysFriction = sideFriction;

            wheelColliders[i] = wheelCollider;

            if (wheel.gameObject.name == "RFront_WheelCollider" || wheel.gameObject.name == "LFront_WheelCollider")
            {
                frontWheels.Add(wheelCollider);
            }
        }

        //Headlights
        headlights = new GameObject[2];

        for (int i = 0; i < headlights.Length; i++)
        {
            GameObject lightObject = new GameObject("Headlight");
            Light light = lightObject.AddComponent<Light>();

            light.type = UnityEngine.LightType.Spot;
            light.range = 10f;
            light.spotAngle = 65;
            light.intensity = 5;

            lightObject.SetActive(false);
            light.transform.parent = vehicleChassis.transform;

            float xOffset = transform.GetComponent<BoxCollider>().size.x / 2;

            if (i < 1)
            {
                xOffset = -xOffset;
            }

            light.transform.position = new Vector3(xOffset, 1.5f, transform.GetComponent<BoxCollider>().size.z / 2);
            headlights[i] = lightObject;
        }
        headlightsActive = false;

        cameraController = cam.GetComponent<CameraControl>();

        string[] cameraModeEnums = System.Enum.GetNames(typeof(CameraMode));

        CameraModes[] GroupResize(int size, CameraModes[] toResize)
        {
            CameraModes[] newTbl = new CameraModes[size];
            for (int i = 0; i < cameraModeEnums.Length; i++)
            {
                string modeName = cameraModeEnums[i];

                CameraModes newObj = new CameraModes(modeName, false);
                newTbl[i] = newObj;
            }

            return newTbl;
        }
        CameraModeData = GroupResize(cameraModeEnums.Length, CameraModeData);

        currentCameraMode = CameraMode.Default;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Respawn vehicle
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //Headlights
            ToggleHeadlights();
        }

        if (currentCameraMode != CameraMode.Watch)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                //Rear view of vehicle
                cameraController.RearView(true);
            }

            if (Input.GetKeyUp(KeyCode.T))
            {
                cameraController.RearView(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
           for (int i = 0; i < CameraModeData.Length; i++)
            {
                CameraModes mode = CameraModeData[i];

                CameraMode GetEnum(CameraModes newMode)
                {
                    //Use type casting
                    CameraMode parsedEnum = (CameraMode)System.Enum.Parse(typeof(CameraMode), newMode.name);
                    return parsedEnum;
                }

                if (mode.name == currentCameraMode.ToString())
                {
                    if ((i + 1) < CameraModeData.Length)
                    {
                        currentCameraMode = GetEnum(CameraModeData[i + 1]);
                    }
                    else
                    {
                        currentCameraMode = GetEnum(CameraModeData[0]);
                    }
                    cameraController.CameraModeUpdated(currentCameraMode);
                    break;
                }
            }
        }

        speedText.text = "MPH: " + (Mathf.Round(curVehicleSpeed) * 15);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Update functions
        UpdateWheels();
        if (canDrive)
        {
            VehicleMovement(verticalInput, horizontalInput);
        }

        float currentCameraTilt = verticalInput * (horizontalInput * cameraTilt);

        if (verticalInput < 0)//Halve the camera rotation when driving backwards for better effect.
        {
            currentCameraTilt = currentCameraTilt / 2;
        }

        //Camera movement
        Quaternion newRot = Quaternion.Euler(0, currentCameraTilt, 0);

        cameraController.UpdateCamera(vehicleChassis, newRot, tiltSpeed);

        UpdateUI();
    }

    void ToggleHeadlights()
    {
        if (!headlightsActive)
        {
            headlightsActive = true;
            foreach (GameObject light in headlights)
            {
                light.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject light in headlights)
            {
                light.SetActive(false);
            }
            headlightsActive = false;
        }
        print(headlightsActive);
    }

    void UpdateWheels()
    {

    }

    void VehicleMovement(float verticalInput, float horizontalInput)
    {
        float speedTo = vehicleSpeed;

        if (verticalInput < 0)
        {
            speedTo = Mathf.Sqrt(speedTo);
        }

        float currentVelocity = speedTo * verticalInput;//Velocity of the vehicle. Multiplied by speedTo.

        steering = maxSteering * horizontalInput;

        //Motor
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.motorTorque = 8000f * Time.deltaTime * currentVelocity;
        }

        //Steering
        foreach (WheelCollider wheel in frontWheels)
        {
            wheel.steerAngle = steering;
        }
        //print(steering);

        //Fake wheels
        for (int i = 1; i < wheelModels_GameObject.transform.childCount; i++)
        {
            Transform wheel = wheelModels_GameObject.transform.GetChild(i).gameObject.transform;
            if (wheel.name == "LFront_Wheel" || wheel.name == "RFront_Wheel")
            {
                //wheel.transform.rotation = Quaternion.Slerp(from, to, Time.deltaTime);
            }
        }

        curVehicleSpeed = Mathf.Abs(currentVelocity);//Absolute value of currentVelocity.
    }

    void UpdateUI()
    {
        float Round(float num)
        {
            return Mathf.Round(num * 100f) / 100f;
        }

        string debugTxt = "vehicleSpeed = " + vehicleSpeed.ToString()
        + "\n" + "currentSpeed = " + Round(curVehicleSpeed).ToString()
        + "\n" + "Input: "
        + "\n" + Round(horizontalInput).ToString()
        + "\n" + Round(verticalInput).ToString()
        + "\n" + "wheelRot = " + Round(steering).ToString()
        + "\n" + "cameraMode = " + currentCameraMode;

        //uiCanvas.transform.Find("Debug").transform.gameObject.GetComponent<TextMeshProUGUI>().text = debugTxt;
    }
}