using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    [SerializeField] private AnimationClip fallClip;
    [SerializeField] private float fallGravMultiplier = 3f;
    
    private float terminalVelocity = 15;
    private void fallGravity(Rigidbody2D rb) {
        rb.velocity += Vector2.up * Physics.gravity.y * (fallGravMultiplier - rb.gravityScale) * Time.deltaTime;
    }

    public override void FixedUpdateState() {
        fallGravity(rb);

    }
    public override void Enter() {
        animator.Play(fallClip.name);
        //reset gravity
        rb.gravityScale = 2;
    }
    public override void Exit() {
        stateDone = true;
        //rb.velocity = Vector2.zero;
    }
    public override void UpdateState() {
        if (playerVariables.grounded) {
            Exit();
        }


    }
}
