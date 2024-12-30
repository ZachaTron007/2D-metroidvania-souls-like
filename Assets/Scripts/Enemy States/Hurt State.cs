using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    [SerializeField] private float recoveryTime;
    private void Start() {
        
    }
    public override void Enter() {
        if (unitVariables.animations.HurtAnimation.length > recoveryTime) {
            recoveryTime = unitVariables.animations.HurtAnimation.length;
        }
        interuptable = .9f;
        animator.Play(unitVariables.animations.HurtAnimation.name);
        Invoke(nameof(Exit), recoveryTime);
    }

    // Update is called once per frame
    public override void Exit() {
        stateDone = true;
        
    }
    public override void FixedUpdateState() {
        rb.linearVelocity = Vector2.zero;
    }
    
}
