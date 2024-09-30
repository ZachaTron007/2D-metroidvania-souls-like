using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : State
{
    [SerializeField] private AnimationClip idelClip;
    public override void UpdateState() {
        
    }
    public virtual void FixedUpdateState() { }
    public override void Enter() {
        
        animator.Play(idelClip.name);
        rb.velocity = Vector2.zero;
    }
    public virtual void Exit() { }
}
