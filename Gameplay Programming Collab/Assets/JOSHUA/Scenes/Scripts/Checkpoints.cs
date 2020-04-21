using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject ground;
    Respawn r; 
    
    public Vector3 transform_new = Vector3.zero;
    public Vector3 rotate_new = Vector3.zero;

    void Start()
    {
        r = ground.GetComponent<Respawn>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            r.new_transform = transform_new;
            r.new_rotation = rotate_new;
            Destroy(gameObject);
        }
    }
}
