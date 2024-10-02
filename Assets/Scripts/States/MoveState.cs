using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    [SerializeField] private AnimationClip runClip;
    [SerializeField] private float moveSpeed = 250;
    
    public override void Enter() {
        animator.Play(runClip.name);
    }
}
