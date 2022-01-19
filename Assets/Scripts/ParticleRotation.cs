using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    public Vector3 speed;
    //public Transform playerTrack;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(speed * Time.deltaTime);
        //transform.position = playerTrack.position;

    }
}
