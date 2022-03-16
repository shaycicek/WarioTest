using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, 2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.gameObject.layer.ToString() == "18" || collisionInfo.collider.gameObject.layer.ToString() == "9") // ?? Enemy Ateþiyle Çarpýþma ??
        {
            gameObject.SetActive(false);
            ObjectPool.Instance.pooledObjects.Enqueue(this.gameObject); // DENEME
            // Queue ya burada !geri sok! -- size kontrol et 
        }
        
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            gameObject.SetActive(false);
            Debug.Log("spawn routine has been activated");
            yield return new WaitForSeconds(spawnInterval);

        }
    }

}
