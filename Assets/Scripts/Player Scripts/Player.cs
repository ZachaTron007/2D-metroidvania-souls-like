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
    [SerializeField] private GameObject attack;
    [SerializeField] private BoxCollider2D attackHitBox;

    //movements
    [SerializeField] private float moveSpeed = 250;
    private float direction = 1;
    private int dashDirection = 1;
    //Jumps
    [SerializeField] private float fallGravMultiplier = 3f;
    [SerializeField] private float jumpGravMultiplier = 20f;
    private float terminalVelocity = 15;
    //dash
    private float dashCool = 0.7f;
    private float dashCount;

    //kyoteTime
    private float kyoteTime = 0.1f;
    private float kyoteTimer;
    //attacks
    private int attackNum;
    private float attackTime;
    public float AttackTime;
    //states
    private bool rightWallTouch = false;
    private bool leftWallTouch = false;
    private bool leftGround = false;
    private bool falling = false;
    //components
    Animator animatior;
    public Rigidbody2D rb;
    private Vector2 moveVetcor;
    private SpriteRenderer sr;
    //scrupts
    private dashScript Dash;
    private Health1 health;
    private jumpScript jumpScript;
    private wallActionsScript wallActions;
    //buttons
    private KeyCode jump = KeyCode.Space;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode dash = KeyCode.LeftShift;

    private void Awake() {
        
        //get the input system
        playerControls = new PlayerControls();
        //scripts
        Dash = GetComponent<dashScript>();
        health = GetComponent<Health1>();
        jumpScript = GetComponent<jumpScript>();
        wallActions = GetComponent<wallActionsScript>();
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
        //attacks
        if (attack) {
            //attackHitBox = attack.getComponent<BoxCollider2D>();
            
        }
        
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
        if (!Dash.dashing) {
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
                jumpScript.grounded = false;
                kyoteTimer = 0;

            }
        }

        //the iniatal jump, you need to be in kyote time or on the ground
        dashCount += Time.deltaTime;
        /*DASH ACTIVATE*/
        if (Input.GetKeyDown(dash) && dashCount >= dashCool && !Dash.dashing) {
            StartCoroutine((Dash.dash(direction, rb)));

        }

        //if you arent on the ground
        if (!jumpScript.grounded) {
            /*WALL JUMP ACTIVATE*/
            if (falling) {
                /*WALL SLIDE ACTIVATE*/
                if (rightWallTouch && Input.GetKey(right) || leftWallTouch && Input.GetKey(left)) {
                    wallActions.wallSliding = true;
                    
                }
                if (wallActions.wallSliding) {
                    wallActions.wallSlide(rb);

                    if (Input.GetKeyDown(jump)) {
                        dashDirection = rightWallTouch ? -1 : 1;
                        //jumpScript.Jump(rb);
                        StartCoroutine(wallActions.WallJump(dashDirection, rb));

                    }
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


        } else {
            /*RESETS SHIT*/
            /*GROUNDED*/
            if (Input.GetKeyDown(jump)) {
                jumpScript.Jump(rb);
            }

        }

        if (Input.GetMouseButtonDown(0) && attackTime > 0.25f && !Dash.dashing && !wallActions.wallSliding) {
            attackNum++;
            // Loop back to one after third attack
            if (attackNum > 3)
                attackNum = 1;

            // Reset Attack combo if time since last attack is too large
            if (attackTime > 1.0f) { 
                attackNum = 1;
                Attack();
            }
            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animatior.SetTrigger("Attack" + attackNum);

            // Reset timer
            attackTime = 0.0f;
        }
        attackTime += Time.deltaTime;


        animatior.SetBool("falling", falling);
        animatior.SetBool("WallSliding", wallActions.wallSliding);
        animatior.SetBool("wallJumping", wallActions.wallJump);
        /*SETS RUN ANIMATION VAR*/
        animatior.SetFloat("moving", Mathf.Abs(moveVetcor.x));
        animatior.SetBool("notJumping", jumpScript.grounded);
    }
    private void FixedUpdate() {
        //actualy moves you left and right using physics
        if(!wallActions.wallJump&&!Dash.dashing) {
            /*MOVE LEFT AND RIGHT*/
            rb.velocity = new Vector2(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }






    private void Attack() {
        Invoke("AttackEnd", AttackTime);
        attackHitBox.enabled=true;
    }

    private void AttackEnd() {
        attackHitBox.enabled=false;
    }
/*COLLISIONS AND ENABLE AND DISABLE*/
    private void groundSensorEnter(Collider2D other) {
        jumpScript.grounded = true;
        wallActions.wallSliding = false;
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
        wallActions.wallSliding = false;
        leftGround = false;

    }
    private void leftWallSensorEnter(Collider2D other) {
        leftWallTouch = true;
    }
    private void leftWallSensorExit(Collider2D other) {
        leftWallTouch = false;
        wallActions.wallSliding = false;
        leftGround = false;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Spike") {
            Debug.Log("hit");
            health.Damage(1);
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
