using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    public GameObject pickup_effect;
    GameObject player;

    private float speed = 10.0f;
    private float dist = 3.0f;

    public int score_increase = 1;

    private void Start()
    {
        player = GameObject.Find("Playable_Character");
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

    private int actions()
    {
        Instantiate(pickup_effect, transform.position, transform.rotation);
        Destroy(gameObject);

        return score_increase;
    }
}
