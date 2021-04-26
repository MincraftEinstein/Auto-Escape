using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPathing : MonoBehaviour
{
    //private Transform[] pathPoints;
    //public GameObject path;
    public float movmentSpeed;
    private float timer;
    private static Vector3 currentPos;
    //private int currentPoint;
    private Vector3 startPos;
    public GameObject movePoint;
    private bool isOnRoad;
    private List<GameObject> roadParts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //pathPoints = path.GetComponentsInChildren<Transform>();
        CheckPoint();
    }

    void CheckPoint()
    {
        timer = 0;
        //if (movePoint.transform.position.y > 0 || movePoint.transform.position.y < 0)
        //{
        currentPos = /*pathPoints[currentPoint]*/movePoint.transform.position;
        //}
        //else if (movePoint.transform.position.y == 0)
        //{
        //currentPos.x = movePoint.transform.position.x;
        //currentPos.y = 0;
        //currentPos.z = movePoint.transform.position.z;
        //}
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (roadParts.Count <= 0)
        {
            isOnRoad = false;
        }
        else
        {
            isOnRoad = true;
        }
        if (isOnRoad)
        {
            timer += Time.deltaTime * movmentSpeed;
            if (transform.position != currentPos)
            {
                transform.position = Vector3.Lerp(startPos, currentPos, timer);
            }
            else
            {
                //if (currentPoint < pathPoints.Length - 1)
                //{
                //currentPoint++;
                CheckPoint();
                //}
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            if (!roadParts.Contains(collision.gameObject))
            {
                roadParts.Add(collision.gameObject);
            }
            transform.rotation = collision.gameObject.transform.rotation;
            //Debug.Log(collision.gameObject.transform.rotation);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            roadParts.Remove(collision.gameObject);
        }
    }
}