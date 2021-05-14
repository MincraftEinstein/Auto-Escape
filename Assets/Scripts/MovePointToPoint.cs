using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointToPoint : MonoBehaviour
{
    Transform[] path;
    public GameObject pathObject;
    public float movmentSpeed;
    private float timer;
    private static Vector3 currentPos;
    private int currentPoint;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        path = pathObject.GetComponentsInChildren<Transform>();
        CheckPos();
    }

    void CheckPos()
    {
        timer = 0;
        transform.rotation = path[currentPoint].rotation;
        currentPos = path[currentPoint].position;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * movmentSpeed;
        if (transform.position != currentPos)
        {
            transform.position = Vector3.Lerp(startPos, currentPos, timer);
        }
        else
        {
            if (currentPoint < path.Length - 1)
            {
                currentPoint++;
                CheckPos();
            }
            else if (currentPoint >= path.Length - 1)
            {
                currentPoint = 0;
                CheckPos();
            }
        }
    }
}
