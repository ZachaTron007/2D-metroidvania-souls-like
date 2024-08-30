using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallActionsScript : MonoBehaviour
{
    //wallJump
    private float wallJumpSpeed = 100;
    private float wallJumpDuration = 0.3f;
    private float stayDuration = 0.2f;
    private float wallJumpCount;
    //wallslide 
    private float wallSlideSpeed = 3;
    public bool wallSliding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void wallSlide(Rigidbody2D rb) {
        wallSliding = true;
        rb.gravityScale = 0;
        rb.velocity = -Vector2.up * wallSlideSpeed;
    }
    /*
    private void WallJump() {
        if (wallJumpCount == 0) {
            //Jump();
        }
        if (wallJumpCount < wallJumpDuration) {
            rb.velocity = new Vector2(dashDirection * wallJumpSpeed * Time.fixedDeltaTime, rb.velocity.y);
            wallJumpCount += Time.deltaTime;
        } else if (direction == dashDirection)
            rb.velocity = new Vector2(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

        if (wallJumpCount >= wallJumpDuration && rb.velocity.y <= -stayDuration) {
            wallJumpCount += Time.deltaTime;
            wallJumpCount = 0;
            wallJump = false;
        }
    }*/

    public IEnumerator wallJump() {

        yield return null;
    }
}
