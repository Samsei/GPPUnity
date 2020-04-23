using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplat2 : MonoBehaviour
{
    public Vector3 t3;
    public Vector3 t4;

    public static float movespeed; //how quickly the platform moves
    public Vector3 Direction1;      //direction of travel
    public void Start()
    {
        movespeed = 4f;
    }

    public void Update()
    {
        if (transform.position.x > t3.x || transform.position.x < t4.x)
            Direction1 = Direction1 * -1;

        transform.Translate(Direction1 * movespeed * Time.deltaTime);
    }
}