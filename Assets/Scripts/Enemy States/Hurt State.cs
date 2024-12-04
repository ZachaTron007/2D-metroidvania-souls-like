using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    [SerializeField] protected AnimationClip damageClip;
    [SerializeField] private float recoveryTime;
    private void Start() {
        
    }
    public override void Enter() {
        if (damageClip.length > recoveryTime) {
            recoveryTime = damageClip.length;
        }
        interuptable = .9f;
        animator.Play(damageClip.name);
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
