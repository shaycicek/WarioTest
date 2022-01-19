using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject player;
    public GameObject bulletParnt;
    public GameObject bullet;
    public float pHealth = 100f;
    public Transform turret;
    private float timer = 0;

    // List
    public List<Soldier> fsoldierList;
    private List<GameObject> bulletList = new List<GameObject>();

    public LayerMask whatIsEnemy, whatIsFriend;
    // States
    public float attackRange;
    public bool enemyInAttackRange;
    // Attack
    public GameObject projectile;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;




    // List oluþtur!!!

    // Start is called before the first frame update
    void Start()
    {
        fsoldierList = new List<Soldier>();     
    }

    // Update is called once per frame
    void Update()
    {
        //shootBullet();
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        if (enemyInAttackRange) attackEnemy(); // && Input.GetKeyDown("space")
        //attackEnemy();
        clearBullets();

        if (pHealth<= 0)
        {
            Destroy(gameObject);
        }
        

    }

     void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.gameObject.layer.ToString() == "17") // ?? Enemy Ateþiyle Çarpýþma ??
        {
            pHealth--;
            //Debug.Log("Player Health = "+pHealth);
        }
        
    }

    private void attackEnemy()
    {
        if (!alreadyAttacked)
        {
            //Debug.Log("player attacked ");
            turret.transform.LookAt(FindClosestEnemy().transform.position);
            GameObject proj = Instantiate(projectile, turret.position, Quaternion.identity);
            proj.gameObject.layer = 19;
            proj.gameObject.transform.parent = bulletParnt.transform;
            bulletList.Add(proj);  // Filling the bulletlist
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.AddForce(turret.transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(turret.transform.up * 8f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

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

    public void addToList(Soldier soldier)
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


}
