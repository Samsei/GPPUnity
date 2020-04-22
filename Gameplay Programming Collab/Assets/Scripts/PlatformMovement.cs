using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject[] waypoints;
    int current = 0;
    public float speed;
    public bool isMoving;
    float wPRadius = 1;

    private void FixedUpdate()
    {
        Movement();
    }

    //Moves the platform between a set number of points and then resets the target point
    void Movement()
    {
        if (isMoving)
        {
            if (Vector3.Distance(waypoints[current].transform.position, transform.position) < wPRadius)
            {
                current++;

                if (current >= waypoints.Length)
                {
                    current = 0;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
        }
    }

    ////Parents the player to the platform on enter
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        other.transform.parent = gameObject.transform;
    //        Debug.Log("Parented");
    //    }
    //}

    ////Unparents the player to the platform on enter
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        other.transform.parent = null;
    //        Debug.Log("Un-parented");
    //    }
    //}
}
