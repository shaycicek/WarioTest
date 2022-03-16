using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateControl : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isFiringHash;
    public Player player;
    public gameManager gManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isFiringHash = Animator.StringToHash("isFiring");
        gManager = gameManager.instance;
        player = gManager.p1;
    }

    // Update is called once per frame
    void Update()
    {

        bool isWalking = animator.GetBool(isWalkingHash);
        //bool isFiring = animator.GetBool(isFiringHash);
        bool forwardPressed = Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d");
        // if player presses w key
        if (player.getFiringBool())
        {
            //animator.SetBool(isFiringHash, true);
        }else if(!player.getFiringBool())
        {
            //animator.SetBool(isFiringHash, false);
        }
        if(!isWalking && forwardPressed)
        {
            // set the "isWalking" boolean to true
            animator.SetBool(isWalkingHash, true);
        }
        // if player releases the w key
        if(isWalking && !forwardPressed)
        {
            // set the "isWalking" boolean to false
            animator.SetBool(isWalkingHash, false);
        }
    }
}
