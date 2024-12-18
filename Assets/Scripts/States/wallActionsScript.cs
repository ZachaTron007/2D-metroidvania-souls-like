using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallActionsScript : JumpScript
{
    //wallJump
    private float wallJumpSpeed = 100;
    private float wallJumpDuration = 0.3f;
    //wallslide 
    private float wallSlideSpeed = 3;
    public bool wallSliding = false;
    public bool wallJump = false;
    // Start is called before the first frame update  
    public void wallSlide(Rigidbody2D rb) {
        wallSliding = true;
        rb.gravityScale = 0;
        rb.linearVelocity = -Vector2.up * wallSlideSpeed;
    }

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
    }

}
