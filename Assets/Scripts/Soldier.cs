using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    //Variables
    public Player p1;
    public Transform fturret;
    public GameObject Player;
    public SphereCollider pCollider;
    public Transform FriendlybulletParent;
    public Rigidbody rb;
    public Transform mTarget;
    public ParticleSystem particle;
    public Vector3 size;
    public Vector3 pos;
    public int sHealth = 4;

    private float timer = 0;
    private List<GameObject> bulletList = new List<GameObject>();

    public float sightRange, attackRange;
    public bool enemyInAttackRange;

    public LayerMask whatIsEnemy, whatIsFriend,whatIsGround;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public GameObject projectile;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;

    // DENEME @@@@@
    public NavMeshAgent agent;
    // DENEME
    void Awake()
    {
        size= new Vector3(pCollider.radius, 0, pCollider.radius);
        
        //DENEME  @@@@@@@@@@@@ Baþlangýç
        mTarget = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        // DENEME SON @@@@@@
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        clearBullets();
        //if the soldier transformed to Friendly Soldier
        if (gameObject.layer == 12)
        {
            MoveWithPlayer();
            if (enemyInAttackRange) AttackWithPlayer();
        } else if (gameObject.layer == 7)
        {
            Patroling();
        }
        // Caný sýfýrlanýnca ölür
        if (sHealth <= 0)
        {

            p1.fsoldierList.Remove(this);
            Destroy(gameObject);
            /*for(int i = 0; i<p1.fsoldierList.Count; i++)
            {
                if(p1.fsoldierList[i] == this)
                {
                    p1.fsoldierList.Remove(this);
                }
            } */
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {   
        //Base ile çarpýþma
        if (collisionInfo.collider.gameObject.layer.ToString() == "13")
        {
//Collision Start

            // Particle Radius 
            ParticleSystem.ShapeModule psShape = particle.shape;
            psShape.radius= psShape.radius+psShape.radius*0.05f;

            //Collider Radius
            pCollider.radius = pCollider.radius+pCollider.radius*0.05f;

            //Layer Change(Soldier --> Friendly Soldier)
            gameObject.layer = 12;
            gameObject.tag = "FriendlySoldier";

            //List ekleme
            p1.addToList(this);
            Debug.Log(p1.fsoldierList.Count + " = Fsoldierlist count");

            // Parent deðiþikliði - ??|child count|??
            gameObject.transform.parent = Player.transform;  
            pos = Random.insideUnitSphere * pCollider.radius; //new Vector3(Player.transform.childCount, 0 ,Player.transform.childCount ); //Random.Range?
            pos = new Vector3(pos.x, 0.5f, pos.z);
        }
        // Bullet ile çarpýþma
        if (collisionInfo.collider.gameObject.layer.ToString() == "17" && gameObject.layer  == 12) 
        {
            sHealth--;
        }
// Collision end
    }
    void MoveWithPlayer()
    {
            //agent.speed = 15;
            transform.LookAt(mTarget.position);
            transform.position = mTarget.position + pos;
            //agent.SetDestination(mTarget.position);      //???????Moving weird?????       
            transform.rotation = Quaternion.identity;
            //Rb.movePosition ??        
    }

    void Patroling()
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

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void AttackWithPlayer()
    {
        if (!alreadyAttacked)
        {
            fturret.transform.LookAt(FindClosestEnemy().transform.position);
            GameObject proj = Instantiate(projectile, fturret.position, Quaternion.identity);
            proj.gameObject.layer = 19;
            proj.gameObject.transform.parent = FriendlybulletParent.transform;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.AddForce(fturret.transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(fturret.transform.up * 8f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
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

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    void clearBullets()
    {
        timer += Time.deltaTime;
        timer = (int)System.Math.Ceiling(timer);
        if (bulletList != null)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                if ((timer / 100) % 3 == 0)
                {
                    Destroy(bulletList[i]);
                    Debug.Log("Destroyed");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
