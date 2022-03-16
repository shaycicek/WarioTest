using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float spawnInterval = 10;


    //Parents
    public GameObject enemyParent;
    public GameObject soldierParent;
    // Transforms
    public Transform Enemy;    
    //Prefabs
    public GameObject EnemyPrefab;
    public GameObject SoldierPrefab;

    public float Radius = 1;
    public float timer = 0;
    Vector3 randomPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(spawnRoutine));
        randomPos = Random.insideUnitCircle * Radius;
        randomPos.y = 1;

    }

    // Update is called once per frame
    void Update()
    {
        /* timer += Time.deltaTime;
        timer = (int)System.Math.Ceiling(timer);
        
        if ((timer/100)%10 == 0)
        {
            SpawnEnemyAtRandom();
            SpawnSoldierAtRandom();
            //ItemPrefab.transform.parent = Enemy.parent;
        } */

    }
     
    void SpawnEnemyAtRandom()
    {
        randomPos =Random.insideUnitCircle * Radius;
        randomPos.y = 1;
        GameObject clone;
        clone = Instantiate(EnemyPrefab, randomPos, Quaternion.identity);
        clone.transform.parent = enemyParent.transform;
        clone.SetActive(true);
    }

    void SpawnSoldierAtRandom()
    {
        randomPos = Random.insideUnitCircle * Radius;
        randomPos.y = 1;
        GameObject soldierClone;
        soldierClone = Instantiate(SoldierPrefab, randomPos, Quaternion.identity);
        soldierClone.transform.parent = soldierParent.transform;
        soldierClone.SetActive(true);
        
    }


    private IEnumerator spawnRoutine()
    {
        while (true)
        {
            SpawnEnemyAtRandom();
            SpawnSoldierAtRandom();
            yield return new WaitForSeconds(spawnInterval);

        }
    }

}
