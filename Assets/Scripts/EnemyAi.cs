using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool ;
    public ParticleSystem deathEff;
    public ParticleSystem shotEff;
    public GameObject bulletsParent;
    public NavMeshAgent agent;
    public Player p1;
    public LayerMask whatIsGround, whatIsPlayer;
    public int aiHealth;
    private float timer;

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
    public bool isDead;

    //Animation
    public Animator animator;
    int isWalkingHash;
    int isDeadHash;
    

    private void Start()
    {
        //Animator Initialize
        animator = GetComponentInChildren<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isDeadHash = Animator.StringToHash("isDead");

        timer = 0;
        aiHealth = 10;
        p1 = gameManager.instance.p1;
        bulletsParent = gameManager.instance.enemyBulletParent;
        StartCoroutine(nameof(attackRoutine));
        objectPool = ObjectPool.Instance;
        agent = GetComponent<NavMeshAgent>();

    }
    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        
        //Check for sight and attack range
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!isDead)
        {
            if (!playerInsightRange && !playerInAttackRange)
            {
                Patroling();
            }
            if (playerInsightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            if (playerInAttackRange && playerInsightRange)
            {
                AttackPlayer();
            }

        }


        if (aiHealth <= 0)
        {
            isDead = true;
            animator.SetBool(isDeadHash, true);
            deathEff.gameObject.SetActive(true);

            timer += Time.deltaTime;
            
            if (timer > 0.5)
            {
                Destroy(this.gameObject);
            }


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer.ToString() == "19") // ?? Collides with Friendly Bullet ??
        {
            aiHealth--;
        }
    }

    private void Patroling()
    {
        animator.SetBool(isWalkingHash, true);
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
        //animator.SetBool(isWalkingHash, false);
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
        animator.SetBool(isWalkingHash, true);
        agent.SetDestination(p1.getPos().position);
        animator.SetBool(isWalkingHash, false);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        

        // Eðer soldierList boþsa playera saldýracak deðilse önce soldier a saldýracak!
        if (p1.getSoldierListSize() !=0)
        {
            Debug.Log("Soldier Count = "+p1.getSoldierListSize());
            transform.LookAt(FindClosestSoldier().transform);
            foreach (Soldier temp in p1.fsoldierList)
            {
                Debug.Log(temp);
            }

        } else
        {
            transform.LookAt(p1.getPos().position);
        }

        if (!alreadyAttacked)
        {
            // Attack code
            shotEff.gameObject.SetActive(false);
            shotEff.gameObject.SetActive(true);
            GameObject obj = objectPool.GetPooledObject();
            obj.gameObject.layer = LayerMask.NameToLayer("enemyBullet");
            obj.transform.position = turret.transform.position;
            obj.GetComponent<Rigidbody>().AddForce(transform.forward * 32f, ForceMode.Impulse);

            alreadyAttacked = true;

        } 
    }



    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private IEnumerator attackRoutine()
    {
        while (true)
        {
            ResetAttack();
            yield return new WaitForSeconds(timeBetweenAttacks);

        }
    }


    public GameObject FindClosestSoldier()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("FriendlySoldier"); //kontrol -- checksphere -- overlap sphere  //p1.fsoldierlist ile check et
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
