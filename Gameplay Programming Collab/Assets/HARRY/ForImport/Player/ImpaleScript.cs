using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpaleScript : MonoBehaviour
{
    //https://www.youtube.com/watch?v=VVqbsWbl5Zg - Tutorial that helped with this

    public bool debug;

    public GameObject impaleHitFX;
    public int maximumLength;
    public float separation, spawnDelay, damageDelay, height, radius, force, yOffset;
    public LayerMask layerMask;
    
    [HideInInspector]
    public GameObject fxParent;
    [HideInInspector]
    public int currentLength;
    private bool isLast = false, hasSpawnedNext = false, hasDamaged = false;
    private ParticleSystem ps;
    private float spawnDelayTimer, damageDelayTimer;

    private void Start()
    {
        //Check for first impale obj
        if(currentLength == 0)
        {
            fxParent = gameObject;
        }

        ps = GetComponent<ParticleSystem>();

        //Set delays
        spawnDelayTimer = spawnDelay;
        damageDelayTimer = damageDelay;
    }

    private void Update()
    {
        if(spawnDelayTimer <= 0 && !hasSpawnedNext)
        {
            CreateImpaleObject();
        }

        if(spawnDelayTimer > 0)
        {
            spawnDelayTimer -= Time.deltaTime;
        }

        if(spawnDelayTimer > 0)
        {
            damageDelayTimer -= Time.deltaTime;
        }

        if(damageDelayTimer <= 0 && !hasDamaged)
        {
            AOEDamage();
        }

        if(!ps.isPlaying && isLast)
        {
            Destroy(fxParent);
        }
    }

    void CreateImpaleObject()
    {
        if(currentLength < maximumLength)
        {
            var rayCastPosition = transform.position + transform.forward * separation;
            rayCastPosition.y += height;
            RaycastHit hit;

            if(Physics.Raycast(rayCastPosition, Vector3.down, out hit, height + 1, layerMask))
            {
                if(hit.transform != transform)
                {
                    var spawnLoc = hit.point;
                    spawnLoc.y += yOffset;
                    hasSpawnedNext = true;
                    var obj = Instantiate(gameObject, transform);
                    obj.transform.position = spawnLoc;
                    obj.transform.rotation = transform.rotation;
                    var impale = obj.GetComponent<ImpaleScript>();
                    impale.currentLength = currentLength + 1;
                    impale.maximumLength = maximumLength;
                    impale.fxParent = fxParent;
                }
            }

            else
            {
                isLast = true;
            }
        }

        else
        {
            isLast = true;
        }
    }

    void AOEDamage()
    {
        hasDamaged = true;

        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider col in enemiesHit)
        {
            Rigidbody enemy = col.GetComponent<Rigidbody>(); 

            if(enemy != null)
            {
                //enemy.AddForce(Vector3.up * force, ForceMode.VelocityChange);

                //Spawns a hit effect at hit location
                //var fx = Instantiate(impaleHitFX, enemy.transform.position, Quaternion.identity);

                //Destroy(fx, 1.5f);

                //Apply enemy damage
                col.GetComponent<SlimeAI>().TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(debug && hasDamaged)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
