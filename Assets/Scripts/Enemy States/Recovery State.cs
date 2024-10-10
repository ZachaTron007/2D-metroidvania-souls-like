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
        animator.Play(idelAniamtion.name);
        rb.velocity = Vector2.zero;
        interuptable = true;
        Invoke("Exit", recoverTime);
    }

    public override void Exit() {
        recovering=false;
        stateDone = false;
    }
}
