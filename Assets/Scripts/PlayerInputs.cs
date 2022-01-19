using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    private float forwardInput;
    public float ForwardInput
    {
        get { return forwardInput; }
    }

    private float rotationInput;
    public float RotationInput
    {
        get { return rotationInput; }
    }
    // Start is called before the first frame update
    public Camera camera;

    private Vector3 reticlePosition;

    // Update is called once per frame
    void Update()
    {
        if (camera)
        {
            HandleInputs();

        }
    }

    protected virtual void HandleInputs()
    {
        forwardInput = Input.GetAxis("Vertical");
        rotationInput = Input.GetAxis("Horizontal");
        


    }
}
