using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineCrossed : MonoBehaviour
{
    [Header("Finish Line")]
    public GameObject finishMenu;
    public GameObject finishLine;

    [Header("Laps")]
    public int numberOfLaps;

    [ReadOnly]
    public int currentLap;

    private void Update()
    {
        if (currentLap >= numberOfLaps)
        {
            if (GetComponent<CarController>())
            {
                GetComponent<CarController>().isBreaking = true;
            }
            else if (GetComponent<PlayerController>())
            {
                GetComponent<PlayerController>().canDrive = false;
            }

            finishMenu.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finishLine)
        {
            if (gameObject.name == "Player")
            {
                Debug.Log(currentLap + " | " + currentLap++);
            }
            currentLap =+ 1;
            Debug.Log("Reached finishline");
        }
    }
}
