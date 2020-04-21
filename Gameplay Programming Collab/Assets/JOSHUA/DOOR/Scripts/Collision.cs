using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    GameObject player;
    MovementController mc;

    Button button;

    void Start()
    {
        player = GameObject.Find("Playable_Character");
        mc = player.GetComponent<MovementController>();
        button = GetComponentInParent<Button>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            button.in_collision = true;
            mc.in_interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            button.in_collision = false;
            mc.in_interactable = false;
        }
    }
}
