using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplat : MonoBehaviour
{
    public Vector3 t1;
    public Vector3 t2;

    public static float movespeed; //how quickly the platform moves
    public Vector3 Direction;      //direction of travel
    public void Start()
    {
        movespeed = 4f;
        Direction = Vector3.right;
    }

    public void Update()
    {
        if (transform.position.x < t2.x || transform.position.x > t1.x)
            Direction = Direction * -1;

        transform.Translate(Direction * movespeed * Time.deltaTime);
    }
}