using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movepoles : MonoBehaviour
{
    public Vector3 t1;
    public Vector3 t2;

    public static float movespeed; //how quickly the platform moves
    public Vector3 Direction;      //direction of travel
    public void Start()
    {
        movespeed = 1f;
        Direction = Vector3.right;
    }

    public void Update()
    {
        if (transform.position.y < t2.y)
            Direction = -Direction;

        if (transform.position.y > t1.y)
            Direction = Vector3.right;

        transform.Translate(Direction * movespeed * Time.deltaTime);
    }
}
