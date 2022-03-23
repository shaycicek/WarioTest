using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem deathEff;
    public bool isDead;
    public ParticleSystem shotEff;
    public bool pFiring;
    public static Player Instance;
    [SerializeField] private ObjectPool objectPool ;
    public int soldierLimit;
    public HealthBar healthBar;
    public GameObject bulletParnt;
    public int pHealth = 150;
    public int curHealth;
    public Transform turret;
    public GameObject restartBut;
    private float timer;

    // List
    public List<Soldier> fsoldierList;
   
    public LayerMask whatIsEnemy, whatIsFriend;
    // States
    public float attackRange;
    public bool enemyInAttackRange;
    // Attack
    bool alreadyAttacked = false;
    public float attackInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        healthBar = gameManager.instance.healthBar;
        bulletParnt = gameManager.instance.FriendlybulletParent;
        soldierLimit = 10;
        curHealth = pHealth;
        healthBar.SetMaxHealth(pHealth);
        objectPool = ObjectPool.Instance;
        StartCoroutine(nameof(attackRoutine));
        fsoldierList = new List<Soldier>();
        
    }
    // Update is called once per frame
    void Update()
    {

        //shootBullet();
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        if (enemyInAttackRange)
        {
            pFiring = true;
            AttackEnemy();
        } else
        {
            pFiring = false;
        }

        //StartCoroutine(nameof(attackRoutine));
        if (curHealth<= 0)
        {
            deathEff.gameObject.SetActive(true);
            timer += Time.deltaTime;
            isDead = true;
            if (timer > 1)
            {
                gameObject.SetActive(false);
                restartBut.SetActive(true);
                Time.timeScale = 0;
                Destroy(gameObject);
                
                
                
            }
        }
        

    }

     void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.gameObject.layer == LayerMask.NameToLayer("enemyBullet"))
        {
            curHealth--;
            healthBar.SetHealth(curHealth);
            //Debug.Log("Player Health = "+curHealth);
        }
        
    }

    private void AttackEnemy()
    {
        if (!alreadyAttacked)
        {
            turret.transform.LookAt(FindClosestEnemy().transform.position);

            //GameObject proj = Instantiate(projectile, turret.position, Quaternion.identity);
            if(objectPool == null)
            {
                Debug.LogError("HATA");
            }
            shotEff.gameObject.SetActive(false);
            shotEff.gameObject.SetActive(true);
            GameObject obj = objectPool.GetPooledObject();
            obj.transform.position = turret.transform.position;
            obj.gameObject.layer = LayerMask.NameToLayer("friendlyBullet");
            obj.gameObject.transform.parent = bulletParnt.transform;
            obj.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 32f, ForceMode.Impulse);
            
            alreadyAttacked = true;
            
            //Invoke(nameof(ResetAttack), timeBetweenAttacks);
            
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

    public void AddToList(Soldier soldier)
    {
        fsoldierList.Add(soldier);
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

    public Transform getPos()
    {
        Transform obj = gameObject.transform;
        return obj;

    }
    public int getSoldierListSize()
    {
        return fsoldierList.Count;
    }

    public bool getFiringBool()
    {
        return pFiring;
    }
    public bool GetDeadBool()
    {
        return isDead;
    }

    private IEnumerator attackRoutine()
    {
        while (true)
        {
            ResetAttack();
            yield return new WaitForSeconds(attackInterval);

        }
    }


}
