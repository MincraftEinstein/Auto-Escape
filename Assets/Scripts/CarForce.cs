using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarForce : MonoBehaviour
{
    //Transform[] path;
    //public GameObject pathObject;
    //private int nodeIndex;
    //Transform target;
    //Vector3 dir;
    //float speed;
    //public float maxSpeed;
    //public float Acceleration;
    //Rigidbody RB;

    // Start is called before the first frame update
    //void Start()
    //{
    //    path = pathObject.GetComponentsInChildren<Transform>();
    //    Debug.Log(path.Length);
    //    RB = GetComponent<Rigidbody>();
    //}

    // Update is called once per frame
    //void Update()
    //{
    //if (path[0] != null)
    //{

    //}

    //if (transform.position == path[nodeIndex].position)
    //{
    //    transform.rotation = path[nodeIndex].rotation;
    //}
    //}

    //private void FixedUpdate()
    //{
    //    if (speed < maxSpeed)
    //    {
    //        RB.AddForce(Vector3.forward * Acceleration);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "Cube" + (nodeIndex + 1))
    //    {
    //        transform.rotation = path[nodeIndex].rotation;
    //        nodeIndex++;
    //    }
    //}

    public float movmentSpeed;
    //private float timer;
    private static Vector3 currentPos;
    private Vector3 startPos;
    private Rigidbody RB;
    public GameObject movePoint;
    private bool isOnRoad;
    private float speed;
    public float maxSpeed;
    private List<GameObject> roadParts = new List<GameObject>();

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        CheckPoint();
    }

    void CheckPoint()
    {
        //timer = 0;
        currentPos = movePoint.transform.position;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        speed = RB.velocity.z;
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
            //timer += Time.deltaTime * movmentSpeed;
            if (transform.position != currentPos)
            {
                //transform.position = Vector3.Lerp(startPos, currentPos, timer);
                //Debug.Log(name + " | start = " + startPos + " | end = " + currentPos);
                if (speed < maxSpeed)
                {
                    RB.AddRelativeForce(Vector3.right * movmentSpeed);
                    Debug.Log("r = " + transform.rotation);
                    Debug.Log("f = " + Vector3.right);
                }
            }
            else
            {
                CheckPoint();
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
            StartCoroutine(SetRotation(collision));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            roadParts.Remove(collision.gameObject);
        }
    }

    IEnumerator SetRotation(Collision collision)
    {
        //for (float f = 0; f < RB.velocity.z; f++)
        //{
        //    RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, RB.velocity.z - 0.1F);
        //}
        yield return new WaitForSeconds(Random.Range(0.13F, 0.20F));
        transform.rotation = collision.gameObject.transform.rotation;
        //RB.AddForce(collision.transform.forward * movmentSpeed);

        //Vector3 pos = movePoint.transform.position;
        //pos = new Vector3(pos.x + 1, pos.y, pos.z);
        //RB.AddForce(Vector3.right * movmentSpeed);
        //yield return new WaitForSeconds(2);
        //pos = new Vector3(pos.x - 1, pos.y, pos.z);
    }
}
