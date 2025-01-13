using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideScript : State
{
    //wallJump
    //private float wallJumpSpeed = 100;
    //private float wallJumpDuration = 0.3f;
    //wallslide 
    private float wallSlideSpeed = 3;
    public bool wallJump = false;
    [SerializeField] private GameObject dust;

    public override void Enter() {
        base.Enter();
        animator.Play(unitVariables.animations.wallSlideAnimation.name);
    }
    // Start is called before the first frame update  
    public override void UpdateState() {
        rb.gravityScale = 0;
        rb.linearVelocity = -Vector2.up * wallSlideSpeed;/*
        slideDust = Instantiate(dust, this.transform);
        slideDust.transform.rotation = rotation;
    */}
    /*
    public IEnumerator WallJump(float dashDirection, Rigidbody2D rb) {
        wallSliding = false;
        wallJump = true;
        Jump();
        Invoke("wallJumpReset", wallJumpDuration);
        while (wallJump) {
            rb.linearVelocity = new Vector2(dashDirection * wallJumpSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
            yield return null;
        }
        yield return null;
    }

    private void wallJumpReset() {
        wallJump = false;
    }*/

}
