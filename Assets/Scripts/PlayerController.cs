using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]

public class PlayerController : MonoBehaviour
{

    public float pSpeed = 30f ;
    public float pRotationSpeed = 1f;
    private Rigidbody rb;
    private PlayerInputs input;
    public bool isMoving;
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

        // Move SidewaysS
        if(input.RotationInput!=0 || input.ForwardInput != 0)
        {
            isMoving = true;
            move = new Vector3(input.RotationInput, 0, input.ForwardInput);
            rb.MovePosition(transform.position + transform.TransformDirection(move) * Time.deltaTime * pSpeed);
        }
        else
        {
            isMoving = false;
        }

        
          
        //Rotate

        /* Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (pRotationSpeed * input.RotationInput * Time.deltaTime));
        rb.MoveRotation(wantedRotation); 
        */
    }

}
