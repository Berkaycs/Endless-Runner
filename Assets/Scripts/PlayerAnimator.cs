using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //private const string CAN_JUMP = "CanJump";
    //private const string CAN_DOUBLE_JUMP = "CanDoubleJump";
    private const string IS_SLIP = "IsSlip";
    private const string IS_DEAD = "IsDead";

    [SerializeField] private PlayerMovement player;
    public Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        /*
        animator.SetBool(CAN_JUMP, player.isOnGround);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("double jump anim");
            animator.SetBool(CAN_DOUBLE_JUMP, player.doubleJump);
        }
        */

        if (player.isOnGround && Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Slip anim works");
            animator.SetBool(IS_SLIP, true);
        }
        else
        {
            animator.SetBool(IS_SLIP, false);
        }
    }
}
