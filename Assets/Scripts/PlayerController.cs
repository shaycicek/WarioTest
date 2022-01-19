using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]

public class PlayerController : MonoBehaviour
{

    public float pSpeed = 15f ;
    public float pRotationSpeed = 20f;
    private Rigidbody rb;
    private PlayerInputs input;
    Vector3 move = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputs>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb && input)
        {
            HandleMovement();            
        }



    }


    public virtual void HandleMovement()
    {
        //Move Forward
        /* Vector3 wantedPosition = (transform.position) + (transform.forward* input.ForwardInput * pSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition); */

        // Move Sideways
        move = new Vector3(input.RotationInput , 0  , input.ForwardInput);
        rb.MovePosition(transform.position + transform.TransformDirection(move));


          
        //Rotate

        /* Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (pRotationSpeed * input.RotationInput * Time.deltaTime));
        rb.MoveRotation(wantedRotation); 
        */
    }

}
