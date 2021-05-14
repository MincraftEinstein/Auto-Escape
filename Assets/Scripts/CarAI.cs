using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CarAI : MonoBehaviour
{
    private float speed = 0.09F;
    private bool isOnRoad = true;
    public GameObject cube;
    private List<GameObject> roadParts = new List<GameObject>();

    private IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0;
        float rate = 1.0F / time;
        while (i < 1.0F)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Road"))
        {
            isOnRoad = true;
            transform.rotation = collision.gameObject.transform.rotation;
            if (!roadParts.Contains(collision.gameObject))
            {
                roadParts.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            //Debug.Log("Stopped colliding with road");
            roadParts.Remove(collision.gameObject);
        }
    }

    void Update()
    {
        if (roadParts.Count <= 0)
        {
            isOnRoad = false;
        }
        
        if (isOnRoad)
        {
            GameObject GO = roadParts[roadParts.Count - 1];
            Vector3 GOScale = GO.transform.localScale;

            float GOScaleX = GOScale.x / 2;
            float GOScaleX2 = -GOScale.x / 2;

            float GOScaleZ = GOScale.z / 2;
            float GOScaleZ2 = -GOScale.z / 2;

            //if (transform.position.x == GO.transform.position.x/* + (GOScaleZ / 2)*/)
            //{
            //    Debug.Log("Testing");
            //}
            if (transform.position.x >= (GO.transform.position.x + GOScaleX))
            {
                //transform.Rotate(0, 90, 0);
                Debug.Log("If 1");
            }
            else if (transform.position.x <= (GO.transform.position.x + GOScaleX2))
            {
                transform.Rotate(0, 90, 0);
                Debug.Log("If 2");
            }
            else if (transform.position.z >= (GO.transform.position.z + GOScaleZ))
            {
                Debug.Log("If 3");
            }
            else if (transform.position.z <= (GO.transform.position.z + GOScaleZ2))
            {
                Debug.Log("If 4");
            }
        }
        StartCoroutine(updatePos());
    }

    IEnumerator updatePos()
    {
        if (isOnRoad)
        {
            Vector3 pointB = cube.transform.position; // This is the end pos
            Vector3 pointA = transform.position; // This is the start pos
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, speed)); // Moves the car from pointA to pointB
        }
    }
}
