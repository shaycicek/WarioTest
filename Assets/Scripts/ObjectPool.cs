using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public bool willGrow;
    public Queue<GameObject> pooledObjects;
    [SerializeField]private GameObject objectPrefab;
    [SerializeField]private int poolsize;

    public static ObjectPool Instance;

    private void Awake()
    {
        Instance = this;
        pooledObjects = new Queue<GameObject>();

        for(int i =0; i< poolsize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.transform.parent = gameManager.instance.enemyBulletParent.transform;
            obj.SetActive(false);
            pooledObjects.Enqueue(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        /* //Lazýmsa büyült!!
        GameObject obj = pooledObjects.Dequeue();
        // çaðýrýlan objeyi sýradan çýkardýk
        obj.SetActive(true);     
        //objemizi sahnede görünür hale getirdik
        pooledObjects.Enqueue(obj); //
        Debug.Log("Pool count = " + pooledObjects.Count);
        // ve tekrar sýranýn sonuna ekledik */



        GameObject[] pobj = pooledObjects.ToArray();
        for (int i = 0; i<pooledObjects.Count; i++)
        {
            if(!pobj[i].activeInHierarchy)
            {
                pobj[i] = pooledObjects.Dequeue();
                pobj[i].SetActive(true);
                pooledObjects.Enqueue(pobj[i]);
                return pobj[i];
            }
        }

        if(willGrow)
        {
            GameObject objec = Instantiate(objectPrefab);
            objec.transform.parent = gameManager.instance.enemyBulletParent.transform;
            pooledObjects.Enqueue(objec);
            return objec;
        }

        //return obj;
        return null;
    }
}
