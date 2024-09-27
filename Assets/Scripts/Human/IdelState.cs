using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : State
{
    [SerializeField] private AnimationClip idelClip;
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void Enter() {
        animator.Play(idelClip.name);
        stateDone = true;
    }
    public virtual void Exit() { }
}
