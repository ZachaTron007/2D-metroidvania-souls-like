using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryState : State
{
    [SerializeField] private float moveSpeed = 250;
    [SerializeField] private AnimationClip idelAniamtion;
    [SerializeField] private AnimationClip walkAniamtion;
    [SerializeField] private float recoverTime;
    public override void Enter() {
        animator.Play(idelClip.name);
        rb.velocity = Vector2.zero;
        interuptable = true;
        invoke("Exit", recoverTime);
    }

    public override void Exit() {
        stateDone = false;
    }
}
