using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryState : State
{
    [SerializeField] protected AnimationClip idelAniamtion;
    [SerializeField] protected float recoverTime = 1;
    public override void Enter() {
        animator.Play(idelAniamtion.name);
        rb.linearVelocity = Vector2.zero;
        interuptable = .6f;
        Invoke("DoneRecovering", recoverTime);
    }

    private void DoneRecovering() {
        unitVariables.isRecovering = false;
        Exit();
    }

    public override void UpdateState() {
        rb.linearVelocity = Vector2.zero;
    }
}
