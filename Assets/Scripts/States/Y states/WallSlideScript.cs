using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideScript : RbVelocityLerp
{
    //wallJump
    //private float wallJumpSpeed = 100;
    //private float wallJumpDuration = 0.3f;
    //wallslide 
    [SerializeField] private float maxWallSlideSpeed = 3;
    public bool wallJump = false;
    [SerializeField] private GameObject dust;
    private GameObject slideDust;
    public Quaternion rotation;

    public override void Enter() {
        base.Enter();
        rb.linearVelocity = Vector3.zero;
        rb.gravityScale = 0;
        ResetLerp(startSpeed: rb.linearVelocity.y, totalTime: totalTime, Vector2.up);
        animator.Play(unitVariables.animations.wallSlideAnimation.name);
    }

    public override void Exit() {
        base.Exit();
        rb.gravityScale = HelperFunctions.regularGavityScale;
    }

    protected override float GetTargetSpeed() {
        return maxWallSlideSpeed;
    }

    protected override void FinishedLerping() {
        

    }
}
