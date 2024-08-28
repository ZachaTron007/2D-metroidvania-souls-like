using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    private PlayerControls playerControls;
    private InputAction move;
    //colliders
    [SerializeField] private Sensors groundSensor;
    [SerializeField] private Sensors rightWallSensor;
    [SerializeField] private Sensors leftWallSensor;
    private BoxCollider2D mainCollider;
    private CapsuleCollider2D clipCollider;
    //movements
    [SerializeField] private int health = 50;
    [SerializeField] private float moveSpeed = 250;
    private float direction = 1;
    private int dashDirection = 1;
    //Jumps
    [SerializeField] private float fallGravMultiplier = 3f;
    [SerializeField] private float jumpGravMultiplier = 20f;
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    private float terminalVelocity = 15;
    //wallJump
    private float wallJumpSpeed = 100;
    private float wallJumpDuration = 0.3f;
    private float stayDuration = 0.2f;
    private float wallJumpCount;
    //hurt
    private float colorChangeSpeed = 0.7f;
    private float colorCounter;
    private Vector4 hurtColor = new Vector4(255, 62, 62, 255);
    private bool hurt = false;
    //wallslide and dash
    private float wallSlideSpeed = 3;
    private float dashSpeed = 6;
    private float dashCool = 0.7f;
    private float dashCount;
    private float dashduration = 0.13f;
    private float dashDurCount;
    //states
    private bool grounded = false;
    private bool wallSliding = false;
    private bool wallJump = false;
    private bool rightWallTouch = false;
    private bool leftWallTouch = false;
    private bool dashing = false;
    private bool leftGround = false;
    private bool attacking = false;
    private bool falling = false;
    //components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveVetcor = new Vector2(0,0);


    /*FUNCTIONS*/
    private void wallSlide() {
        wallSliding = true;
        rb.gravityScale = 0;
        rb.velocity = -Vector2.up * wallSlideSpeed;
    }

    private void WallJump() {
        if (wallJumpCount == 0) {
            Jump();
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
    }

    public void Jump() {
        //makes the y component change
        grounded = false;
        rb.velocity = Vector2.up * jumpVelocity;
        rb.gravityScale = 2;
    }
    public void Dash() {
        if (dashDurCount < dashduration) {
            rb.velocity += Vector2.right * dashSpeed * direction;
            dashDurCount += Time.deltaTime;
        }
        if (dashDurCount >= dashduration) {
            dashDurCount = 0;
            dashCount = 0;
            dashing = false;
        }
    }
    IEnumerator DDash() {

        yield return null;
    }
    public void Hurt() {
        if (colorCounter >= colorChangeSpeed && colorCounter < colorChangeSpeed * 2) {
            sr.color = hurtColor / 255;
            colorCounter += Time.deltaTime;
        } else if (colorCounter >= colorChangeSpeed * 2) {
            //colorCounter = 0;
            sr.color = Color.white;
        } else {
            sr.color = Color.white;
            colorCounter += Time.deltaTime;
        }
    }
    public void die() {
        Destroy(gameObject);
    }


    
    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
    }
    private void OnDisable() {
        move = playerControls.Player.Move;
        move.Disable();
    }
}
