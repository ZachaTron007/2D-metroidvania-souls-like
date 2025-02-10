using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideScript : LerpingInhertedClass
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
    private RbVelocityLerp wallSlideMovment;
    protected void Start() {
        clip = unitVariables.animations.wallSlideAnimation;
    }
    public override void Enter() {
        base.Enter();
        rb.linearVelocity = Vector3.zero;
        wallSlideMovment = new RbVelocityLerp(startSpeed: rb.linearVelocity.y, targetSpeed: maxWallSlideSpeed, totalTime: totalTime, Vector2.up,rb, curve);
        rb.gravityScale = 0;
    }

    public override void Exit() {
        base.Exit();
        rb.gravityScale = HelperFunctions.regularGavityScale;
    }
}
