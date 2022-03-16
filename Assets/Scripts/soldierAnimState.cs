using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soldierAnimState : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    public Soldier soldier;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);


        if (soldier.soldierisWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (!soldier.soldierisWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }


    }
}
