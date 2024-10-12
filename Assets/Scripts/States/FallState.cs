using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    [SerializeField] private AnimationClip fallClip;
    [SerializeField] private float fallGravMultiplier = 3f;
    
    private float terminalVelocity = 15;
    private void fallGravity() {
        rb.velocity += Vector2.up * Physics.gravity.y * (fallGravMultiplier - rb.gravityScale) * Time.deltaTime;
    }

    public override void FixedUpdateState() {
        if (rb.velocity.y > -terminalVelocity) {
            fallGravity();
        } else {
            rb.velocity =new Vector2(rb.velocity.x, -terminalVelocity);
        }
    }
    public override void Enter() {
        animator.Play(fallClip.name);
        //reset gravity
        rb.gravityScale = 2;
    }
    public override void UpdateState() {
        if (unitVariables.grounded) {
            Exit();
        }


    }
}
