using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    GameObject player;
    AbilityTimer at;

    [SerializeField] float timer = 15.0f;
    [SerializeField] int collectable_type = 1;

    // 1 = speed
    // 2 = double jump

    private void Start()
    {
        player = GameObject.Find("Playable_Character");
        at = player.GetComponent<AbilityTimer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            at.updateVariables(timer, collectable_type);
        }
    }
}
