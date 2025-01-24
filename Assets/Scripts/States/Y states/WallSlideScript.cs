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
    private float wallSlideSpeed = 3;
    [SerializeField] private float maxWallSlideSpeed = 3;
    public bool wallJump = false;
    [SerializeField] private GameObject dust;
    private GameObject slideDust;
    public Quaternion rotation;

    public override void Enter() {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        ResetLerp(startSpeed: 0, totalTime: totalTime);
        base.Enter();
        animator.Play(unitVariables.animations.wallSlideAnimation.name);
    }
    // Start is called before the first frame update  
    public override void UpdateState() {
        
        //rb.linearVelocity = -Vector2.up * wallSlideSpeed;
        //slideDust = Instantiate(dust, transform.position, rotation, transform);
        
    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        rb.AddForce(Vector2.down * movement);
    }

    public override void Exit() {
        base.Exit();
        rb.gravityScale = HelperFunctions.regularGavityScale;
    }

    protected override float GetTargetSpeed() {
        dir = Vector2.down;
        
        return maxWallSlideSpeed;
    }

    protected override void FinishedLerping() {
        

    }
}
