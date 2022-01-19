using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //Parents
    public GameObject enemyParent;
    public GameObject soldierParent;
    // Transforms
    public Transform Enemy;

    //Clones
    public GameObject aiClone;
    public GameObject clone;
    public GameObject soldierClone;
    
    //Prefabs
    public GameObject EnemyPrefab;
    public GameObject SoldierPrefab;

    public float Radius = 1;
    public float timer = 0;
    Vector3 randomPos;

    // Start is called before the first frame update
    void Start()
    {
        
        randomPos = Random.insideUnitCircle * Radius;
        randomPos.y = 1;
        aiClone = Instantiate(EnemyPrefab, randomPos, Quaternion.identity);
        aiClone.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer = (int)System.Math.Ceiling(timer);
        
        if ((timer/100)%10 == 0)
        {
            SpawnEnemyAtRandom();
            SpawnSoldierAtRandom();
            //ItemPrefab.transform.parent = Enemy.parent;
        }

    }
     
    void SpawnEnemyAtRandom()
    {
        randomPos =Random.insideUnitCircle * Radius;
        randomPos.y = 1;
        clone = Instantiate(aiClone, randomPos, Quaternion.identity);
        clone.transform.parent = enemyParent.transform;
        clone.SetActive(true);
    }

    void SpawnSoldierAtRandom()
    {
        randomPos = Random.insideUnitCircle * Radius;
        randomPos.y = 1;
        soldierClone = Instantiate(SoldierPrefab, randomPos, Quaternion.identity);
        soldierClone.transform.parent = soldierParent.transform;
        soldierClone.SetActive(true);
    }

}
