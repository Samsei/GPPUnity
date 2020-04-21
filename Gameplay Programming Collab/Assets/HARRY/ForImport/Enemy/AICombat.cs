using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : MonoBehaviour
{
    public bool attacked;
    int attacking = 0;

    PlayerCombat player;
    Rigidbody rigidbody;
    public Animator animator;
    public ParticleSystem playerHitEffect;

    public float attackRate = 1f;
    float nextAttackTime = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        rigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    //Player enters combat area, combat mechanics take place
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                //Begins CombatPause coroutine in SlimeAI
                attacked = true;

                //How many times slime can attack per second, set to once
                nextAttackTime = Time.time + 1f / attackRate;

                StartCoroutine(DamagePlayer());
            }
        }
    }

    //Player leaves combat area
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            attacked = false;
            StopCoroutine(DamagePlayer());
        }
    }

    //Processes when player is damaged
    IEnumerator DamagePlayer()
    {
        while (attacked)
        {
            player.currentHealth--;
            Instantiate(playerHitEffect, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Quaternion.identity);
            animator.SetTrigger("Hit");
            player.StopAllCoroutines();
            rigidbody.AddForce(-transform.forward * 600f);
            rigidbody.detectCollisions = false;

            yield return new WaitForSeconds(0.5f);

            rigidbody.detectCollisions = true;
            rigidbody.velocity = Vector3.zero;
        }
    }
}
