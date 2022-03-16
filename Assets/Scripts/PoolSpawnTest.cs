using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSpawnTest : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1;
    
    [SerializeField] private ObjectPool objectPool = null;

    private Vector3 randomPos;
    private float Radius = 200f;
    void Start()
    {
        StartCoroutine(nameof(SpawnRoutine));
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            
            var obj = objectPool.GetPooledObject();
            randomPos = Random.insideUnitCircle * Radius;
            randomPos.y = 1;
            obj.transform.position = randomPos;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
