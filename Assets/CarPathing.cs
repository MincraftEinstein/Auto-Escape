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
        //movePoint.transform.position = new Vector3(transform.position.x + 0.75F, transform.position.y, transform.position.z);
        CheckPoint();
    }

    void CheckPoint()
    {
        timer = 0;
        //if (movePoint.transform.position.y > 0 || movePoint.transform.position.y < 0)
        //{
        currentPos = /*pathPoints[currentPoint]*/movePoint.transform.position;
        //Debug.Log(gameObject.name + " pos = " + transform.position + "ID = " + GetInstanceID());
        //Debug.Log("localpos = " + movePoint.transform.localPosition + " globalpos = " + movePoint.transform.position);
        //Debug.Log("this = " + name + " | point = " + movePoint.name + " | parent = " + movePoint.transform.parent.gameObject.name);
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
        if (isOnRoad/*true*/)
        {
            //float left = roadParts[roadParts.Count - 1].transform.position.z - (roadParts[roadParts.Count - 1].transform.localScale.z / 2);
            //float right = roadParts[roadParts.Count - 1].transform.position.z - (-roadParts[roadParts.Count - 1].transform.localScale.z / 2);
            timer += Time.deltaTime * movmentSpeed;
            if (transform.position != currentPos)
            {
                transform.position = Vector3.Lerp(startPos, currentPos, timer);
                //Debug.Log(name + " | start = " + startPos + " | end = " + currentPos);
            }
            else
            {
                //if (currentPoint < pathPoints.Length - 1)
                //{
                //currentPoint++;
                CheckPoint();
                //}
            }
            //Vector3 offset = roadParts[roadParts.Count - 1].transform.right * (transform.localScale.y / 2) * -1;
            //Vector3 pos = roadParts[roadParts.Count - 1].transform.position + offset;
            //float num = 0.00125F * movmentSpeed;
            //float num = 0.005F;
            //float num = 0.003F;
            //float roadZL = roadParts[roadParts.Count - 1].transform.position.z;
            //float roadZR = -roadParts[roadParts.Count - 1].transform.position.z;
            //if (transform.position.z <= left/*(roadParts[roadParts.Count - 1].transform.position.z / 2)*/)
            //{
            //    //Debug.Log("calculated pos " + pos);
            //    //Debug.Log("Z pos " + roadParts[roadParts.Count - 1].transform.position.z);
            //    //Debug.Log("On the left edge");
            //    movePoint.transform.Translate(Vector3.back * Time.deltaTime * movmentSpeed * num);
            //    //transform.Rotate(0, 1, 0);
            //}
            //if (transform.position.z >= right)
            //{
            //    //Debug.Log("On the right edge");
            //    movePoint.transform.Translate(Vector3.forward * Time.deltaTime * movmentSpeed * num);
            //}

            //if (transform.localPosition.z > roadZL)
            //{
            //    //movePoint.transform.Translate(Vector3.back * Time.deltaTime * movmentSpeed * num);

            //}
            //if (transform.localPosition.z < roadZR)
            //{
            //    movePoint.transform.Translate(Vector3.forward * Time.deltaTime * movmentSpeed * num);
            //}

            //Vector3 roadPos = roadParts[roadParts.Count - 1].transform.position;
            //if (IfNotBetweenNum(transform.rotation.y, 25, -25))
            //{
            //    if (transform.position.x <= left)
            //    {
            //        transform.position = new Vector3(left, transform.position.y, transform.position.z);
            //    }
            //    if (transform.position.x >= right)
            //    {
            //        transform.position = new Vector3(right, transform.position.y, transform.position.z);
            //    }
            //}
            //if (IfNotBetweenNum(transform.rotation.y, 150, -150))
            //{
            //    if (transform.position.z <= left)
            //    {
            //        transform.position = new Vector3(transform.position.x, transform.position.y, left);
            //    }
            //    if (transform.position.z >= right)
            //    {
            //        transform.position = new Vector3(transform.position.x, transform.position.y, right);
            //    }
            //}

            //Vector3 offset = roadParts[roadParts.Count - 1].transform.forward * (roadParts[roadParts.Count - 1].transform.localScale.z / 2F) * -1F;
            //Vector3 pos = roadParts[roadParts.Count - 1].transform.localPosition + offset; //This is the position
            //Vector3 offset2 = roadParts[roadParts.Count - 1].transform.forward * (-roadParts[roadParts.Count - 1].transform.localScale.z / 2F) * -1F;
            //Vector3 pos2 = roadParts[roadParts.Count - 1].transform.localPosition + offset2; //This is the position
            //Debug.Log(pos);
            ////Debug.Log("car pos" + transform.InverseTransformPoint(pos));
            //if (transform.position.z <= pos.z)
            //{
            //    transform.position = new Vector3(transform.position.x, transform.position.y, pos.z);
            //}
            //else if (transform.position.z >= pos2.z)
            //{
            //    transform.position = new Vector3(transform.position.x, transform.position.y, pos2.z);
            //}
            //float pos = roadParts[roadParts.Count - 1].transform.position + (transform.right * -transform.localScale / 2);
        }
    }

    //bool IfNotBetweenNum(float num1, float largerNum, float smallerNum)
    //{
    //    if (num1 > largerNum && num1 < smallerNum)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            if (!roadParts.Contains(collision.gameObject))
            {
                roadParts.Add(collision.gameObject);
            }
            StartCoroutine(SetRotation(collision));
            //Debug.Log(collision.gameObject.transform.rotation);
            //gameObject.AddComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            roadParts.Remove(collision.gameObject);
            //Destroy(GetComponent<Rigidbody>());
        }
    }

    IEnumerator SetRotation(Collision collision)
    {
        yield return new WaitForSeconds(Random.Range(0.13F, 0.20F));
        transform.rotation = collision.gameObject.transform.rotation;
    }
}