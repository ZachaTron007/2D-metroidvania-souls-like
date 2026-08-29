using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    [SerializeField] private float recoveryTime;
    private void Start() {
        clip = unitVariables.animations.HurtAnimation;
    }
    public override void Enter() {
        base.Enter();
        if (unitVariables.animations.HurtAnimation.length > recoveryTime) {
            recoveryTime = unitVariables.animations.HurtAnimation.length;
        }
        interuptable = .9f;
        animator.Play(unitVariables.animations.HurtAnimation.name);
        Invoke(nameof(Exit), recoveryTime);
    }

    public override void FixedUpdateState() {
        base.FixedUpdateState();
        rb.linearVelocity = Vector2.zero;
    }
    
}
