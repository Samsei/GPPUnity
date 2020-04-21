using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    public NavMeshAgent navMesh;

    [Header("Movement Settings")]
    public float timeForNewPath;
    private float targetX;
    private float targetZ;

    [Header("Detection Settings")]
    public Transform player;
    public float combatPause;

    [Header("Attributes")]
    public int maxHealth;
    public int currentHealth;
    public Material outOfCombatMaterial;
    public Material inCombatMaterial;
    public Material hitMaterial;
    public ParticleSystem deathParticle;
    private Material currentMaterial;
    public GameObject toBeInstantiated;
    public HealthBar healthBar;
    public float randX1;
    public float randX2;
    public float randZ1;
    public float randZ2;

    Animator anim;
    Rigidbody rigidbody;
    Renderer renderer;

    bool inCoroutine;
    bool playerInRange;
    
    AICombat combatScript;
    PlayerCombat playerRef;

    float offset;

    private void Start()
    {
        //Finds the NavMesh component attached to the slime
        navMesh = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        combatScript = GameObject.Find("CombatArea").GetComponent<AICombat>();

        navMesh.SetDestination(GetNewPos());

        renderer.material = outOfCombatMaterial;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
        offset = Random.Range(0, 0.5f);

        healthBar.SetHealth(currentHealth);
        //If the player is not in range, patrol as normal
        if (!playerInRange)
        {
            if (!inCoroutine)
            {
                StartCoroutine(Manager());
            }
        }

        if (currentHealth <= 0)
        {
            for (int i = 0; i < 1; i++)
            {
                anim.Play("Death");
                StartCoroutine(Destroy());
            }
        }

        //If the player is within range, start combat mechanics
        else if (playerInRange)
        {
            InCombat();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(Damage());
    }

    //Destruction process
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.8f);

        Vector3 particlePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        InstantiateSlimes();

        Debug.Log("Spawned");

        Instantiate(deathParticle, particlePos, Quaternion.identity);
                

        Destroy(gameObject);
    }

    void InstantiateSlimes()
    {
        Vector3 randOffset = new Vector3(offset, 2, offset);

        for (int j = 0; j < 2; j++)
        {
            Instantiate(toBeInstantiated, transform.position += randOffset, transform.rotation);
        }
    }

    //Checks if the player is within a certain range of the slime, determined via a sphere collider
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerInRange = true;
            renderer.material = currentMaterial;
        }
    }
    
    //When a player enters the range, the slime's material will change to the combat material
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            renderer.material = inCombatMaterial;

            currentMaterial = renderer.material;
        }

    }

    //Checks if the player leaves the detection range, if so the slime continues its patrol
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerInRange = false;
            renderer.material = outOfCombatMaterial;
        }
    }

    //Generates the new location the slime should move towards using a random range of positions within an area
    Vector3 GetNewPos()
    {
        //Generates an X value between 79, 123 on the X axis
        targetX = Random.Range(randX1, randX2);
        //Generates a Z value between 181,226 on the Z axis
        targetZ = Random.Range(randZ1,randZ2);

        Vector3 pos = new Vector3(targetX, 0, targetZ);
        return pos;
    }

    //Manages path changes
    IEnumerator Manager()
    {
        inCoroutine = true;
        yield return new WaitForSeconds(timeForNewPath);
        Movement();
        inCoroutine = false;
    }

    //Handles movement
    void Movement()
    {
        //Sets the destination of the slime to the position generated
        navMesh.SetDestination(GetNewPos());
    }

    //Movement override if player is within range of the slime
    void InCombat()
    {
        //Sets the destination of the slime to the player's position if they are within range
        navMesh.SetDestination(player.position);
    }

    //Changes the slime's material briefly upon hit, highlight effect
    public IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.1f);

        renderer.material = hitMaterial;

        yield return new WaitForSeconds(0.7f);

        renderer.material = inCombatMaterial;
    }

    public void StartDebuffs()
    {
        StartCoroutine(Debuff());
    }

    //Slime debuffs
    public IEnumerator Debuff()
    {
        navMesh.speed = 0;

        //Knocks the slime back
        rigidbody.AddForce(player.transform.forward * 600f);
        rigidbody.detectCollisions = false;

        yield return new WaitForSeconds(0.5f);

        navMesh.speed = 4;

        rigidbody.detectCollisions = true;
        rigidbody.velocity = Vector3.zero;
    }

    public void StartRangedDebuffs(float power, Vector3 bombPos, float range)
    {
        StartCoroutine(RangedDebuff(power, bombPos, range));
        Debug.Log(power);
        Debug.Log(bombPos);
        Debug.Log(range);
    }

    //Slime ranged debuffs
    public IEnumerator RangedDebuff(float power, Vector3 bombPos, float range)
    {
        rigidbody.detectCollisions = false;
        rigidbody.AddExplosionForce(power, bombPos, range, 3.0F);

        yield return new WaitForSeconds(0.5f);
        
        rigidbody.detectCollisions = true;
        rigidbody.velocity = Vector3.zero;
    }
}
