using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
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
    //kyoteTime
    private float kyoteTime = 0.1f;
    private float kyoteTimer;
    //attacks
    private int attackNum;
    private float attackSpeed = 5;
    private float attackTime;
    private float attackReset;
    //states
    private bool grounded = false;
    private bool wallSliding = false;
    private bool wallJump = false;
    private int doubleJump = 0;
    private bool rightWallTouch = false;
    private bool leftWallTouch = false;
    private bool dashing = false;
    private bool leftGround = false;
    private bool attacking = false;
    private bool falling = false;
    //components
    Animator animatior;
    private Rigidbody2D rb;
    private Vector2 moveVetcor;
    private SpriteRenderer sr;
    //buttons
    private KeyCode jump = KeyCode.Space;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode dash = KeyCode.LeftShift;

    private void Awake() {
        //get the input system
        playerControls = new PlayerControls();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        clipCollider = GetComponent<CapsuleCollider2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //listeners for the entering collisions
        //mainCollider.triggerEnter.AddListener(groundSensorEnter);
        groundSensor.triggerEnter.AddListener(groundSensorEnter);
        rightWallSensor.triggerEnter.AddListener(rightWallSensorEnter);
        leftWallSensor.triggerEnter.AddListener(leftWallSensorEnter);
        //listeners for the exiting collisions
        groundSensor.triggerExit.AddListener(groundSensorExit);
        rightWallSensor.triggerExit.AddListener(rightWallSensorExit);
        leftWallSensor.triggerExit.AddListener(leftWallSensorExit);
        jumpVelocity = Mathf.Sqrt(Physics.gravity.y * rb.gravityScale * jumpHeight * -2);
    }

    // Update is called once per frame
    void Update() {
        //horizontal movement
        /*GETS DIRECTION*/
        if (rb.velocity.y != 0) {
            falling = rb.velocity.y < 0;

        } else {
            falling = false;

        }

        //Debug.Log(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        if (!dashing) {
            moveVetcor = move.ReadValue<Vector2>();

        }

        if (moveVetcor.x != 0) {
            direction = moveVetcor.x;
            sr.flipX = direction < 0;

        }

        //sees if you left the ground
        /*KYOTE TIME*/
        if (leftGround) {
            //increases the kyote timer to allow leway
            kyoteTimer += Time.deltaTime;

            if (kyoteTimer >= kyoteTime) {
                //makes it so you caqnt jump after the timer is up
                leftGround = false;
                grounded = false;
                kyoteTimer = 0;

            }
        }

        //the iniatal jump, you need to be in kyote time or on the ground
        dashCount += Time.deltaTime;
        /*DASH ACTIVATE*/
        if (Input.GetKey(dash) && dashCount >= dashCool) {
            dashing = true;

        }

        //if you arent on the ground
        if (!grounded) {
            /*WALL JUMP ACTIVATE*/
            if (rightWallTouch && Input.GetKey(right) || leftWallTouch && Input.GetKey(left)) {

                if (Input.GetKeyDown(jump)) {
                    wallSliding = false;
                    dashDirection = rightWallTouch ? -1 : 1;
                    Jump();
                    wallJump = true;

                }

            }

            if (falling) {
                /*WALL SLIDE ACTIVATE*/
                if (rightWallTouch && Input.GetKey(right) || leftWallTouch && Input.GetKey(left)) {
                    wallSlide();

            } else {
                rb.gravityScale = 2;
                /*FALLING GRAVITY*/
                rb.velocity += Vector2.up * Physics.gravity.y * (fallGravMultiplier - rb.gravityScale) * Time.deltaTime;

            }

                //change colliders
                clipCollider.enabled = true;
                mainCollider.enabled = false;

            }
            if (!falling && !Input.GetKey(jump)) {
                //changes the y velocity by the jump multiplier
                rb.velocity += Vector2.up * Physics.gravity.y * (jumpGravMultiplier - rb.gravityScale) * Time.deltaTime;
                clipCollider.enabled = false;
                mainCollider.enabled = true;

            }

            /*TERMINAL VELOCITY*/
            if (rb.velocity.y < -terminalVelocity) {
                rb.velocity = -Vector2.up * terminalVelocity;

            }

            /*DOUBLE JUMP*/
            if (doubleJump == 0 && Input.GetKeyDown(jump)) {
                Jump();
                doubleJump++;

            }

        } else {
            /*RESETS SHIT*/
            /*GROUNDED*/
            doubleJump = 0;
            
            if (Input.GetKeyDown(jump)) {
                Jump();
            }

        }

        /*DEATH*/
        if (health <=0)
            die(sr);

        if (Input.GetMouseButtonDown(0) && attackTime > 0.25f && !dashing && !wallSliding) {
            attackNum++;

            // Loop back to one after third attack
            if (attackNum > 3)
                attackNum = 1;

            // Reset Attack combo if time since last attack is too large
            if (attackTime > 1.0f)
                attackNum = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animatior.SetTrigger("Attack" + attackNum);

            // Reset timer
            attackTime = 0.0f;
        }
        attackTime += Time.deltaTime;


        animatior.SetBool("falling", falling);
        animatior.SetBool("WallSliding", wallSliding);
        animatior.SetBool("wallJumping", wallJump);
        /*SETS RUN ANIMATION VAR*/
        animatior.SetFloat("moving", Mathf.Abs(moveVetcor.x));
        animatior.SetBool("notJumping", grounded);
    }
    private void FixedUpdate() {
        //actualy moves you left and right using physics
        /*DASHING*/
        if (dashing) {
            Dash();
            
        } else if (wallJump) {
            /*WALL JUMPING*/
            WallJump();
            
        } else
            /*MOVE LEFT AND RIGHT*/
            rb.velocity = new Vector2(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        
    }






    /*FUNCTIONS*/
    private void wallSlide() {
        wallSliding = true;
        rb.gravityScale = 0;
        rb.velocity = -Vector2.up * wallSlideSpeed;
    }

    private void WallJump() {
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
    private void Jump() {
        //makes the y component change
        grounded = false;
        rb.velocity = Vector2.up * jumpVelocity;
        rb.gravityScale = 2;
    }
    private void Dash() {
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
    private void Hurt() {
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
    private void die(SpriteRenderer sr) {
        Destroy(gameObject);
    }


/*COLLISIONS AND ENABLE AND DISABLE*/
    private void groundSensorEnter(Collider2D other) {
        grounded = true;
        wallSliding = false;
    }
    //detects if you are not touching the floor
    private void groundSensorExit(Collider2D other) {
        leftGround = true;
    }
    private void rightWallSensorEnter(Collider2D other) {
        rightWallTouch = true;
    }
    private void rightWallSensorExit(Collider2D other) {
        rightWallTouch = false;
        wallSliding = false;
        leftGround = false;

    }
    private void leftWallSensorEnter(Collider2D other) {
        leftWallTouch = true;
    }
    private void leftWallSensorExit(Collider2D other) {
        leftWallTouch = false;
        wallSliding = false;
        leftGround = false;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Spike") {
            Debug.Log("hit");
            health--;
            hurt = true;
        }
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
