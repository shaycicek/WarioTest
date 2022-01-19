using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public GameObject bulletsParent;
    private List<GameObject> bulletList = new List<GameObject>();
    private float timer = 0;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public GameObject turret;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States

    public float sightRange, attackRange;
    public bool playerInsightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }
    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInsightRange && !playerInAttackRange) Patroling();
        if (playerInsightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInsightRange) AttackPlayer();
        // Destroying the bullets 
        timer += Time.deltaTime;
        timer = (int)System.Math.Ceiling(timer);
        if (bulletList!=null)
        {
            for(int i=0; i<bulletList.Count; i++)
            {
                if ((timer / 100) % 3 == 0)
                {
                    Destroy(bulletList[i]);
                    Debug.Log("Destroyed");
                } 
            }  
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
           agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached

        if(distanceToWalkPoint.magnitude <1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint , -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(FindClosestSoldier().transform);
        //transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Creating the bullet
            GameObject proj = Instantiate(projectile, turret.transform.position, Quaternion.identity);
            proj.transform.parent =bulletsParent.transform;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            bulletList.Add(proj);  // Filling the bulletlist
            // Attack code
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        } 
    }



    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /* public void TakeDamage(int damage)
    {
        health -= damage;
       
        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    } */


    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public GameObject FindClosestSoldier()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("FriendlySoldier");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
