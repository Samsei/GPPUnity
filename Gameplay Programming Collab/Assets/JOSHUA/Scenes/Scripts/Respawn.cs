using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Vector3 new_transform;
    public Vector3 new_rotation;

    GameObject player;
    CharacterController cc;
    public GameObject Test;

    void Start()
    {
        player = GameObject.Find("Playable_Character");
        cc = player.GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entered");

            cc.enabled = false;

            player.transform.rotation = Quaternion.Euler(new_rotation);
            player.transform.position = new_transform;

            cc.enabled = true;
        }
    }
}
