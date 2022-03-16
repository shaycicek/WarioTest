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
    public Rigidbody rbModel;
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
        /*
        if(rb && input)
        {
            HandleMovement();            
        }
        */
        HandleMovement();


    }


    public virtual void HandleMovement()
    {
        transform.LookAt(transform.position + new Vector3(input.RotationInput, 0, input.ForwardInput ));
        move = new Vector3(input.RotationInput, 0, input.ForwardInput).normalized;

        rb.MovePosition(transform.position + move * Time.fixedDeltaTime * pSpeed);

        Vector3 lookTarget = transform.position + move.normalized;
        Vector3 direction = lookTarget - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction, transform.up);

        if (input.RotationInput!=0 || input.ForwardInput != 0)
        {
            isMoving = true;
            
            //rbModel.rotation = rot;

        }
        else
        {
            isMoving = false;
        }
    }

}
