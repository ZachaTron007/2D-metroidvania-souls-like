using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    [SerializeField] AnimationClip damageClip;
    public override void Enter() {
        interuptable = false;
        animator.Play(damageClip.name);
        Invoke("Exit", damageClip.length);
    }

    // Update is called once per frame
    public override void Exit() {
        stateDone = true;
    }
}
