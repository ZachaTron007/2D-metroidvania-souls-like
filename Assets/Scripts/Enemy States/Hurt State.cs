using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    [SerializeField] private AnimationClip damageClip;
    [SerializeField] private float recoveryTime;
    private void Start() {
        if (damageClip.length > recoveryTime) {
            recoveryTime = damageClip.length;
        }
    }
    public override void Enter() {
        interuptable = false;
        animator.Play(damageClip.name);
        Invoke("Exit", recoveryTime);
    }

    // Update is called once per frame
    public override void Exit() {
        stateDone = true;
    }
    public override void UpdateState() {
        base.UpdateState();

    }
}
