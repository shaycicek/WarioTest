using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    
    public PlayerController pCont;
    [SerializeField] private ObjectPool objectPool;
    //public static Soldier instance;
    //Animator
    public Animator animator;
    int isWalkingHash;
    
    //Variables
    public ParticleSystem deathEff;
    public ParticleSystem shotEff;
    public bool soldierisWalking;
    public GameObject soldierBag;
    public GameObject upgradeButton;
    //public NavMeshPath pathh;
    public Player p1;
    public Transform fturret;
    public SphereCollider pCollider;
    public GameObject FriendlybulletParent;
    public ParticleSystem particle;
    public Vector3 pos;
    public int sHealth = 4;
    private float timer = 0;

    //States
    public float sightRange, attackRange;
    public bool enemyInAttackRange;

    public LayerMask whatIsEnemy, whatIsFriend,whatIsGround;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public enum aiState {Stable ,Patroling , Following , Attacking};
    aiState myState; 
    
    // Attack
    public GameObject projectile;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;

    // DENEME @@@@@
    public NavMeshAgent agent;
    // DENEME
    void Start()
    {
        InitializeSoldier();
    }

    // Update is called once per frame
    void Update()
    {
        
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        //if the soldier transformed to Friendly Soldier
        bool isWalking = animator.GetBool(isWalkingHash);

        if (myState == aiState.Following)
        {
            animator.SetBool(isWalkingHash, true);
            MoveWithPlayer();
            if (enemyInAttackRange)
            {
                AttackWithPlayer();
            }
            
        }

        /* if (myState == aiState.Attacking)
        {
            Debug.Log("Attacking Statement");
            AttackWithPlayer();
        } */

        if (myState == aiState.Patroling)
        {
            animator.SetBool(isWalkingHash, true);
            Patroling();
        }



        // Caný sýfýrlanýnca ölür
        if (sHealth <= 0)
        {
            Debug.Log("Soldier dead = "+p1.getSoldierListSize());
            deathEff.gameObject.SetActive(true);
            Debug.Log("deathEff Active");
            timer += Time.deltaTime;
            if (timer > 0.1)
            {
                p1.fsoldierList.Remove(this);
                Destroy(gameObject);
                pCollider.radius = pCollider.radius - pCollider.radius * 0.01960784313725490196078431372549f;
                ParticleSystem.ShapeModule psShape = particle.shape;
                psShape.radius = psShape.radius - psShape.radius * 0.01960784313725490196078431372549f;
                soldierBag.transform.localScale = soldierBag.transform.localScale - soldierBag.transform.localScale * 0.01185770750988142292490118577075f;
            }

        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {   
        //Base ile çarpýþma
        if (collisionInfo.collider.gameObject.layer == LayerMask.NameToLayer("Base"))
        {
            //Collision Start
            if (p1.getSoldierListSize() < p1.soldierLimit)
            {
                MakeSoldierFriendly();                
            }
            else
            {
                upgradeButton.SetActive(true);
            }
        }
        // Bullet ile çarpýþma
        if (collisionInfo.collider.gameObject.layer == LayerMask.NameToLayer("enemyBullet") && gameObject.layer  == LayerMask.NameToLayer("FriendlySoldier")) 
        {
            sHealth--;
        }
// Collision end
    }
    void MoveWithPlayer()
    {
        
        agent.speed = 50;
        agent.acceleration = 1000;
        agent.angularSpeed = 120;
        agent.ResetPath(); 
        //NAVMESH OBSTICLE !!
        agent.SetDestination(p1.getPos().transform.position+new Vector3(0,0,2));
        //agent.CalculatePath(mTarget.position, pathh);
        soldierisWalking = false;
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
        //soldierisWalking = false;
    }

    void AttackWithPlayer()
    {
        if (!alreadyAttacked)
        {
            shotEff.gameObject.SetActive(false);
            shotEff.gameObject.SetActive(true);
            fturret.transform.LookAt(FindClosestEnemy().transform.position);
            GameObject obj = objectPool.GetPooledObject();
            obj.transform.position = fturret.transform.position;
            obj.gameObject.layer = LayerMask.NameToLayer("friendlyBullet");
            obj.GetComponent<Rigidbody>().AddForce(fturret.transform.forward * 32f, ForceMode.Impulse);
            obj.gameObject.transform.parent = FriendlybulletParent.transform;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Coroutine
            

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private IEnumerator attackRoutine()
    {
        while (true)
        {
            ResetAttack();
            yield return new WaitForSeconds(timeBetweenAttacks);

        }
    }

    public bool getisWalking()
    {
        return soldierisWalking;
    }

    public void InitializeSoldier()
    {
        //instance = this;
        animator = GetComponentInChildren<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        pCont = gameManager.instance.pCont;
        soldierisWalking = false;
        //size = new Vector3(pCollider.radius, 0, pCollider.radius);
        StartCoroutine(nameof(attackRoutine));
        objectPool = ObjectPool.Instance;
        soldierBag = gameManager.instance.soldierBag;
        upgradeButton = gameManager.instance.upgradeButton;
        p1 = gameManager.instance.p1;
        particle = gameManager.instance.particle;
        pCollider = p1.GetComponent<SphereCollider>();
        FriendlybulletParent = gameManager.instance.FriendlybulletParent;
        p1.pRadius = particle.shape.radius;  //Particle temp starting size 
        agent = GetComponent<NavMeshAgent>();
        
        if (gameObject.layer == LayerMask.NameToLayer("Soldier"))
        {
            myState = aiState.Patroling;
            
            soldierisWalking = true;
            //soldier yürüme animasyonu ekle
        }
    }

    public void MakeSoldierFriendly()
    {
        myState = aiState.Following;
        // Soldier yürüme animasyonu
        upgradeButton.SetActive(false);
        // Particle Radius 
        ParticleSystem.ShapeModule psShape = particle.shape;
        psShape.radius = psShape.radius + psShape.radius * 0.02f;
        soldierBag.transform.localScale = soldierBag.transform.localScale + soldierBag.transform.localScale * 0.012f;

        //Collider Radius
        pCollider.radius = pCollider.radius + pCollider.radius * 0.02f;

        //Layer Change(Soldier --> Friendly Soldier)
        gameObject.layer = LayerMask.NameToLayer("FriendlySoldier");
        gameObject.tag = "FriendlySoldier";

        //List ekleme
        p1.addToList(this);


        pos = Random.insideUnitSphere * pCollider.radius;
        pos = new Vector3(pos.x, 0.5f, pos.z);
        transform.position = p1.getPos().transform.position + pos;
    }

}
