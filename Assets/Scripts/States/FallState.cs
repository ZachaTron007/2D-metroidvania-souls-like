using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    [SerializeField] private AnimationClip fallClip;
    [SerializeField] private float fallGravMultiplier = 3f;
    
    private float terminalVelocity = 15;
    private void fallGravity() {
        rb.linearVelocity += Vector2.up * Physics.gravity.y * (fallGravMultiplier - rb.gravityScale) * Time.deltaTime;
    }

    public override void FixedUpdateState() {
        if (rb.linearVelocity.y > -terminalVelocity) {
            fallGravity();
        } else {
            rb.linearVelocity =new Vector2(rb.linearVelocity.x, -terminalVelocity);
        }
    }
    public override void Enter() {
        animator.Play(fallClip.name);
        //reset gravity
        rb.gravityScale = 2;
    }
    public override void UpdateState() {
        if (unitVariables.GetGroundedState()) {
            Exit();
        }


    }
}
