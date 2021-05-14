using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform[] path;
    public GameObject pathObject;
    private int nodeIndex;
    Transform target;
    Vector3 dir;
    float speed;
    public float MaxSpeed;
    public float Acceleration;

    // Start is called before the first frame update
    void Start()
    {
        path = pathObject.GetComponentsInChildren<Transform>();
        Debug.Log(path.Length);
    }

    // Update is called once per frame
    void Update()
    {
        //activeNodes = GameObject.FindGameObjectsWithTag("Node");

        if (path[0] != null)
        {
            if (path[nodeIndex].transform.position.z > transform.position.z)
            {
                target = path[nodeIndex].transform;
            }
            else
            {
                nodeIndex++;
            }
        }

        dir = target.transform.position - transform.position;
        dir = dir.normalized;
        //Debug.Log(dir);

        speed = GetComponent<Rigidbody>().velocity.z;
        Time.timeScale = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //nodeIndex = 0;
        Debug.Log(collision.gameObject.name);
    }

    private void FixedUpdate()
    {
        if (speed < MaxSpeed)
        {
            GetComponent<Rigidbody>().AddForce(dir * Acceleration);
            //GetComponent<Rigidbody>().AddForce(new Vector3 (0,0,1) * Acceleration * 100);
        }
    }
}
