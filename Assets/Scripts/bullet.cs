using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
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
            Destroy(gameObject);
        }
        

    }
}
