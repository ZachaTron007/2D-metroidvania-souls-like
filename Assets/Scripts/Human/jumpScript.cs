using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpScript : State
{
    [SerializeField] private AnimationClip jumpClip;
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    public bool grounded = false;
    public int doubleJump = 0;

    // Start is called before the first frame update
    void Start()
    {
        jumpVelocity = Mathf.Sqrt(Physics.gravity.y * 2 * jumpHeight * -2);
    }

    // Update is called once per frame

    public void Jump(Rigidbody2D rb) {
        //wallSliding = false;
        //makes the y component change
        
        playerVariables.grounded = false;
        rb.velocity = Vector2.up * jumpVelocity;
        rb.gravityScale = 2;
        doubleJump += 1;
        Exit();

    }

    public override void UpdateState() {

    }
    public override void Enter() {
        animator.Play(jumpClip.name);
        Jump(rb);
        stateDone = false;
    }
    public override void Exit() {
        playerVariables.stateDone = true;
        //rb.velocity = Vector2.zero;
    }

}
