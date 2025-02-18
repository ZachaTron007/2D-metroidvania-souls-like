using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryState : State
{
    [SerializeField] protected float recoverTime = 1;
    private void Start() {
        clip = unitVariables.animations.runAnimation;
    }
    public override void Enter() {
        base.Enter();
        animator.Play(unitVariables.animations.idelAnimation.name);
        rb.linearVelocity = Vector2.zero;
        interuptable = .6f;
        Invoke("DoneRecovering", recoverTime);
    }

    private void DoneRecovering() {
        unitVariables.isRecovering = false;
        Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
        rb.linearVelocity = Vector2.zero;
    }
}
