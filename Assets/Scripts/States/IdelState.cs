using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdelState : State
{
    [SerializeField] private AnimationClip idelClip;
    
    public override void Enter() {
        animator.Play(idelClip.name);
        rb.linearVelocity = Vector2.zero;
    }
}
