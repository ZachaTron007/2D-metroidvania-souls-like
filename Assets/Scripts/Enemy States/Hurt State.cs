using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    [SerializeField] private AnimationClip damageClip;
    [SerializeField] private float recoveryTime;
    private void Start() {
        
    }
    public override void Enter() {
        if (damageClip.length > recoveryTime) {
            recoveryTime = damageClip.length;
        }
        interuptable = false;
        animator.Play(damageClip.name);
        Invoke("Exit", recoveryTime);
    }

    // Update is called once per frame
    public override void Exit() {
        stateDone = true;
        
    }
    public override void FixedUpdateState() {
        rb.linearVelocity = Vector2.zero;
    }
    
}
