using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    [SerializeField] private float fallGravMultiplier = 3f;
    
    private float terminalVelocity = 15;
    private void fallGravity() {
        unitVariables.unModifiedSpeed += Vector2.up * Physics.gravity.y * (fallGravMultiplier - rb.gravityScale) * Time.deltaTime;
    }

    public override void FixedUpdateState() {
        base.FixedUpdateState();
        if (unitVariables.unModifiedSpeed.y > -terminalVelocity) {
            fallGravity();
        } else {
            unitVariables.unModifiedSpeed = new Vector2(rb.linearVelocity.x, -terminalVelocity);
        }
    }
    public override void Enter() {
        base.Enter();
        animator.Play(unitVariables.animations.fallAniamtion.name);
        //reset gravity
        rb.gravityScale = 2;
    }
    public override void UpdateState() {
        base.UpdateState();
        if (unitVariables.GetGroundedState()) {
            Exit();
        }


    }
}
