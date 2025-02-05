using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdelState : State
{
    protected void Start() {
        clip = unitVariables.animations.idelAnimation;
    }
    public override void Enter() {
        base.Enter();
        
        //animator.Play(unitVariables.animations.idelAnimation.name);
        //rb.linearVelocity = Vector2.zero;
    }
}
