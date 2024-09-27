using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    [SerializeField] private AnimationClip runClip;
    [SerializeField] private float moveSpeed = 250;
    private void run(Rigidbody2D rb) {
        rb.velocity = new Vector2(playerVariables.moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    public override void FixedUpdateState() {
        run(rb);
    }
    public override void Enter() {
        animator.Play(runClip.name);
        stateDone = false;
    }
    public override void Exit() {
        stateDone = true;
        //rb.velocity = Vector2.zero;
    }
}
