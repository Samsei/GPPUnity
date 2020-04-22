using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attributes")]
    public Animator animator;
    public Transform combatSphere;
    public LayerMask enemyLayers;
    public float attackRange;
    public int attackDamage = 1;
    public float attackRate = 1f;
    float nextAttackTimeSword = 0f;
    float nextAttackTimeBomb = 0f;
    float nextAttackTimeSpikes = 0f;
    public float power = 1000;

    [Header("Respawn")]
    GameObject player;
    CharacterController cc;
    public Vector3 new_transform;
    public Vector3 new_rotation;

    [Header("Effects")]
    public ParticleSystem hitEffect;
    public PlayerHealthBar healthBar;
    public Transform respawnPoint;

    [Header("Bomb Stuff")]
    public GameObject explosion;
    public GameObject bomb;
    public Transform bombSpawn;
    public float bombAttackRange;
    public float bombLaunchForce;
    GameObject instantiatedBomb;

    [Header("Impaler")]
    public GameObject impaler;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;

    [Header("Ability UI Stuff")]
    //Sword text
    public Text swordText;
    float swordTime;
    //Bomb text
    public Text bombText;
    float bombTime;
    //Spike text
    public Text spikeText;
    float spikeTime;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Attacking();
        Countdowns();

        healthBar.SetHealth(currentHealth);

        //Checks if player has died
        if (currentHealth == 0)
        {
            Respawn();
        }
    }

    //Respawns player at set location and resets health to full
    void Respawn()
    {
        player = GameObject.Find("Playable_Character");
        cc = player.GetComponent<CharacterController>();

        cc.enabled = false;

        player.transform.rotation = Quaternion.Euler(new_rotation);
        player.transform.position = new_transform;

        cc.enabled = true;

        currentHealth = maxHealth;
    }

    //Attacking processes
    void Attacking()
    {
        //If current game time is >= next available attack which is in this case current time + 1 second
        if (Time.time >= nextAttackTimeSword)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Play attack animation
                animator.SetTrigger("Attack");

                //Start attacking
                StartCoroutine(AttackStart());

                //How many times you can attack per second, set to once
                nextAttackTimeSword = Time.time + 1f / attackRate;
            }
        }

        //Time until next sword attack is available
        swordTime = nextAttackTimeSword - Time.time;

        if (Time.time >= nextAttackTimeBomb)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //Play range attack animation
                animator.SetTrigger("RangeAttack");

                //Start range attack
                StartCoroutine(RangeAttackStart());

                //How many times you can attack per second, set to once every 2
                nextAttackTimeBomb = Time.time + 2.5f / attackRate;
            }
        }

        //Time until next bomb attack is available
        bombTime = nextAttackTimeBomb - Time.time;

        //Current time   time until next attack (current time + delay)
        if (Time.time >= nextAttackTimeSpikes)
        {
            if (Input.GetMouseButtonDown(2))
            {
                //Play range attack animation
                animator.SetTrigger("RangeAttack");

                //Start spike attack
                StartCoroutine(SpikeAttackStart());

                //How many times you can attack per second, set to once every 5 seconds
                //next attack        = time now  + 5s /     1
                nextAttackTimeSpikes = Time.time + 5f / attackRate;
            }
        }

        //Time until next spike attack is available
        spikeTime = nextAttackTimeSpikes - Time.time;
    }

    void Countdowns()
    {
        //Sword cooldown text
        if (swordTime >= 0)
        {
            swordText.text = swordTime.ToString("0");
        }

        //Bomb cooldown text
        if (bombTime >= 0)
        {
            bombText.text = bombTime.ToString("0");
        }

        //Spike cooldown text
        if (spikeTime >= 0)
        {
            spikeText.text = spikeTime.ToString("0");
        }

        //Sword text is empty if it is 0 or less
        if (swordTime <= 0)
        {
            swordText.text = null;
        }

        //Bomb text is empty if it is 0 or less
        if (bombTime <= 0)
        {
            bombText.text = null;
        }

        //Spike text is empty if it is 0 or less
        if (spikeTime <= 0)
        {
            spikeText.text = null;
        }
    }

    public IEnumerator AttackStart()
    {
        yield return new WaitForSeconds(0.5f);

        Attack();
    }

    public IEnumerator RangeAttackStart()
    {
        yield return new WaitForSeconds(0.5f);

        RangeAttack();
    }

    public IEnumerator SpikeAttackStart()
    {
        yield return new WaitForSeconds(0.5f);

        SpikeAttack();
    }

    void Attack()
    {
        //Detect hit enemies
        Collider[] hitEnemies = Physics.OverlapSphere(combatSphere.position, attackRange, enemyLayers);

        //Cause damage to the enemy
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Enemy hit" + enemy.name);

            //Damage the slime
            enemy.GetComponent<SlimeAI>().TakeDamage(attackDamage);

            //Slows the slime on damage
            enemy.GetComponent<SlimeAI>().StartDebuffs();

            //Spawn hit effect on hit enemy position
            Instantiate(hitEffect, new Vector3(enemy.transform.position.x, enemy.transform.position.y - 0.5f, enemy.transform.position.z + -1f), Quaternion.identity);
        }
    }

    void RangeAttack()
    {
        //Insantiates the bomb on range attack initialisation
        instantiatedBomb = Instantiate(bomb, bombSpawn.transform.position, transform.rotation);

        //Sets the force at which the bomb is launched
        Vector3 force = bombLaunchForce * instantiatedBomb.transform.forward;

        //Applies the force to the bomb
        instantiatedBomb.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        instantiatedBomb.GetComponent<Rigidbody>().AddForce(10f * instantiatedBomb.transform.up, ForceMode.Impulse);

        //Begins the bomb firing
        StartCoroutine(BombExplosion());
    }

    void SpikeAttack()
    {
        //Calculates forward direction
        var spawnCalculation = transform.position + transform.forward;

        //Create first impale object
        Instantiate(impaler, bombSpawn.transform.position, transform.rotation);
    }

    //What occurs when a bomb is fired, including damaging nearby enemies
    IEnumerator BombExplosion()
    {
        yield return new WaitForSeconds(2.0f);

        //Detect hit enemies within bomb range
        Collider[] hitEnemies = Physics.OverlapSphere(instantiatedBomb.transform.position, bombAttackRange, enemyLayers);

        //Cause damage to the enemy
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Enemy hit" + enemy.name);

            //Damage the slime
            enemy.GetComponent<SlimeAI>().TakeDamage(attackDamage);

            //Applies ranged debuffs on damage
            enemy.GetComponent<SlimeAI>().StartRangedDebuffs(100f, instantiatedBomb.transform.position, bombAttackRange);

            //Spawn hit effect on hit enemy position
            Instantiate(hitEffect, new Vector3(enemy.transform.position.x, enemy.transform.position.y - 0.5f, enemy.transform.position.z + -1f), Quaternion.identity);
        }

        //Spawns explosion
        Instantiate(explosion, instantiatedBomb.transform.position, Quaternion.identity);

        Destroy(instantiatedBomb);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(combatSphere.position, attackRange);
    }
}
