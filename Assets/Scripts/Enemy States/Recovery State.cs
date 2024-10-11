using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryState : State
{
    [SerializeField] private float moveSpeed = 250;
    [SerializeField] private AnimationClip idelAniamtion;
    [SerializeField] private AnimationClip walkAniamtion;
    [SerializeField] private float recoverTime = 1;
    public override void Enter() {
        animator.Play(idelAniamtion.name);
        rb.velocity = Vector2.zero;
        interuptable = false;
        Invoke("Exit", recoverTime);
    }

    public override void Exit() {
        Debug.Log("Recovered");
        recovering=false;
        stateDone = true;
    }
}
