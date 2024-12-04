using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdelState : State
{
    [SerializeField] private AnimationClip idelClip;

    private void Start() {
    }

    public override void Enter() {
        base.Enter();
        animator.Play(idelClip.name);
        //rb.linearVelocity = Vector2.zero;
    }
}
