using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject testObject;
    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = testObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.Sin(Time.time);
        testObject.transform.position = initialPos + new Vector3(0, offset, 0);
    }
}
