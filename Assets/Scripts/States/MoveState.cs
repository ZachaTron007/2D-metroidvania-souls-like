using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public float moveSpeed = 300;
    
    public override void Enter() {
        animator.Play(unitVariables.animations.runAnimation.name);
    }
}
