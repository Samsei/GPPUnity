using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject pickup_effect;
    GameObject player;
    AbilityTimer at;

    [SerializeField] float timer = 5.0f;
    [SerializeField] int collectable_type = 1;

    // 1 = speed
    // 2 = double jump

    private float speed = 10.0f;
    private float dist = 5.0f;

    private void Start()
    {
        player = GameObject.Find("Playable_Character");
        at = player.GetComponent<AbilityTimer>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < dist)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z), speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actions();
        }
    }

    private void actions()
    {
        Instantiate(pickup_effect, transform.position, transform.rotation);
        at.updateVariables(timer, collectable_type);
        Destroy(gameObject);
    }
}
