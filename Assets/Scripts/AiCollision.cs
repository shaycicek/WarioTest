using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCollision : MonoBehaviour
{
    public GameObject aiEnemy;
    public GameObject enemyCapsule;
    public int aiHealth ;
    // Start is called before the first frame update
    void Start()
    {
        aiHealth = 10;   
    }


    void FixedUpdate()
    {
        if(aiHealth<=0)
        {
            Destroy(aiEnemy);
        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.gameObject.layer.ToString() == "19") // ?? Collides with Friendly Bullet ??
        {
            aiHealth--;
        }


    }




}
