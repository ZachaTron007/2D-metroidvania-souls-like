using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryState : State
{
    [SerializeField] private AnimationClip idelAniamtion;
    [SerializeField] private float recoverTime = 1;
    public override void Enter() {
        animator.Play(idelAniamtion.name);
        rb.linearVelocity = Vector2.zero;
        interuptable = false;
        Invoke("Exit", recoverTime);
    }

    public override void Exit() {
        unitVariables.isRecovering = false;
        stateDone = true;
    }
    public override void UpdateState() {
        rb.linearVelocity = Vector2.zero;
    }
}
