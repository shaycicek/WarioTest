using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootBuller : MonoBehaviour
{

    public GameObject bullet;

    public float bulletSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("Space"))
        {
            GameObject bulletPrefab = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            Rigidbody bulletPrefabRb = bulletPrefab.GetComponent<Rigidbody>();
            bulletPrefabRb.AddForce(Vector3.forward * bulletSpeed, ForceMode.Impulse);

        }
    }
}
