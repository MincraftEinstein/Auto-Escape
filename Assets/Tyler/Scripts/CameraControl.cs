using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject focalPoint;
    private float cameraOffsetHeight = .5f;

    //private bool cameraControlEnabled = false;

    private float rotationRate = 2.5f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private GameObject player;
    private PlayerController playerController;

    private CameraMode currentCameraMode;
    private bool cameraControlEnabled;
    private bool rearView;

    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.Find("FocalPoint");

        transform.eulerAngles = new Vector3(0, yaw, 0);

        player = GameObject.Find("Player");

        playerController = player.GetComponent<PlayerController>();

        currentCameraMode = playerController.currentCameraMode;
        rearView = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentCameraMode)
        {
            case CameraMode.Default:
                cameraControlEnabled = true;
                break;
            case CameraMode.Follow:
                cameraControlEnabled = false;
                break;
            case CameraMode.Watch:
                cameraControlEnabled = false;
                break;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float yRot = mouseX * rotationRate;
            float xRot = mouseY * rotationRate;
            
            yaw += yRot;
            pitch -= xRot;
        }

        if (cameraControlEnabled)
        {
            if (Input.GetKey(KeyCode.Mouse0) || (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Vector3 mousePos = Input.mousePosition;
                    float mouseX = Input.GetAxis("Mouse X");
                    float mouseY = Input.GetAxis("Mouse Y");

                    float yRot = mouseX * rotationRate;
                    float xRot = mouseY * rotationRate;

                    yaw += yRot;
                    pitch -= xRot;
                }
                else
                {
                    //Left/Right
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        yaw -= rotationRate;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        yaw += rotationRate;
                    }

                    //Up/Down
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        pitch += rotationRate;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        pitch -= rotationRate;
                    }
                }
            }
        }
        else
        {
            pitch = 0;
            yaw = 0;
        }
    }

    public void CameraModeUpdated(CameraMode newCameraMode)
    {
        currentCameraMode = newCameraMode;
    }

    public void RearView(bool boolean)
    {
        rearView = boolean;
    }

    public void UpdateCamera(GameObject obj, Quaternion newRot, float tiltSpeed)
    {
        Quaternion newRotation;
        bool updatePosition = true;

        switch (currentCameraMode)
        {
            case CameraMode.Default:
                updatePosition = true;
                break;
            case CameraMode.Follow:
                updatePosition = true;
                break;
            case CameraMode.Watch:
                updatePosition = false;
                break;
        }

        newRotation = Quaternion.Euler(pitch, yaw, 0);
        if (updatePosition)
        {
            focalPoint.transform.position = obj.transform.position + new Vector3(0, cameraOffsetHeight, 0);

            if (rearView)
            {
                focalPoint.transform.rotation = obj.transform.rotation * Quaternion.Euler(0, 180, 0);
            }
            else
            {
                focalPoint.transform.rotation = obj.transform.rotation * newRotation;
            }
        }
        
        Quaternion from = transform.rotation;
        Quaternion to = focalPoint.transform.rotation;

        transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime * tiltSpeed);
        transform.position = Vector3.Lerp(transform.position, focalPoint.transform.position, Time.deltaTime * tiltSpeed);
    }
}
