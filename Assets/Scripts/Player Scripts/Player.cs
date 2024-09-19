using Cinemachine;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    //states
    private bool rightWallTouch = false;
    private bool leftWallTouch = false;
    private bool leftGround = false;
    private bool falling = false;
    
    //[SerializeField] private bool blood = false;

    //components
    public Animator animatior;
    public Rigidbody2D rb;
    private Vector2 moveVetcor;
    private SpriteRenderer sr;
    //scrupts
    [SerializeField] private GameObject attackManager;
    private MeleeAttack melee;
    private dashScript Dash;
    private Health1 health;
    private jumpScript jumpScript;
    private wallActionsScript wallActions;
    //buttons
    private KeyCode jump = KeyCode.Space;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode dash = KeyCode.LeftShift;
    
    //string literals
    private const string ISMOVING = "moving";
    private const string ISFALLING = "falling";
    private const string ISWALLSLIDING = "WallSliding";
    private const string ISWALLJUMPING = "wallJumping";
    private const string NOTJUMPING = "notJumping";
    private const string ATTACKING = "Attack";
    private const string PARRY = "parry";

    private enum State {
        moving,
        falling,
        dashing,
    }

    private void Awake() {
        
        //get the input system
        playerControls = new PlayerControls();
        //scripts
        Dash = GetComponent<dashScript>();
        health = GetComponent<Health1>();
        jumpScript = GetComponent<jumpScript>();
        wallActions = GetComponent<wallActionsScript>();
        melee = attackManager.GetComponent<MeleeAttack>();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        clipCollider = GetComponent<CapsuleCollider2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //listeners for the entering collisions
        //mainCollider.triggerEnter.AddListener(groundSensorEnter);
        //groundSensor.triggerEnter.AddListener(groundSensorEnter);
        //listeners for the exiting collisions
        //groundSensor.triggerExit.AddListener(groundSensorExit);
        //attacks
        
    }

    // Update is called once per frame
    void Update() {
        GroundTouch();
        Debug.Log(GroundTouch());
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
            leftWallTouch = WallTouch(-1);
            rightWallTouch = WallTouch(1);
            /*WALL JUMP ACTIVATE*/
            if (falling) {

                /*WALL SLIDE ACTIVATE*/
                if (rightWallTouch && Input.GetKey(right) || leftWallTouch && Input.GetKey(left)) {
                    wallActions.wallSliding = true;

                }
                if (!rightWallTouch && !leftWallTouch)
                    wallActions.wallSliding = false;
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


            }
            if (!falling && !Input.GetKey(jump)) {
                //changes the y velocity by the jump multiplier
                rb.velocity += Vector2.up * Physics.gravity.y * (jumpGravMultiplier - rb.gravityScale) * Time.deltaTime;
                

            }

            /*TERMINAL VELOCITY*/
            if (rb.velocity.y < -terminalVelocity) {
                rb.velocity = -Vector2.up * terminalVelocity;

            }
            if(rb.velocity.y > 0){
                clipCollider.enabled = true;
                mainCollider.enabled = false;
            } else {
                clipCollider.enabled = false;
                mainCollider.enabled = true;
            }

        } else {
            /*RESETS SHIT*/
            /*GROUNDED*/
            if (Input.GetKeyDown(jump)) {
                jumpScript.Jump(rb);
            }
            health.Block(animatior);
        }
        health.StopBlocking(animatior);
        if (Input.GetMouseButtonDown(0) && attackTime > 0.35f && !Dash.dashing && !wallActions.wallSliding) {
            attackNum++;
            // Loop back to one after third attack
            if (attackNum > 3)
                attackNum = 1;

            // Reset Attack combo if time since last attack is too large
            if (attackTime > 1.0f) { 
                attackNum = 1;
            }
            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animatior.SetTrigger(ATTACKING + attackNum);
            melee.lookDirection.x = direction;
            melee.Attack();

            // Reset timer
            attackTime = 0.0f;
        }
        attackTime += Time.deltaTime;

        
        animatior.SetBool(ISFALLING, falling);
        animatior.SetBool(ISWALLSLIDING, wallActions.wallSliding);
        animatior.SetBool(ISWALLJUMPING, wallActions.wallJump);
        /*SETS RUN ANIMATION VAR*/
        animatior.SetFloat(ISMOVING, Mathf.Abs(moveVetcor.x));
        
        
}
    private void FixedUpdate() {
        
        //actualy moves you left and right using physics
        if (!wallActions.wallJump&&!Dash.dashing) {
            /*MOVE LEFT AND RIGHT*/
            rb.velocity = new Vector2(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }





    /*
    private void Attack() {
        Invoke("AttackEnd", AttackTime);
        attackHitBox.enabled=true;
    }

    private void AttackEnd() {
        attackHitBox.enabled=false;
    }
    */
/*COLLISIONS AND ENABLE AND DISABLE
    private void groundSensorEnter(Collider2D other) {
        jumpScript.grounded = true;
        wallActions.wallSliding = false;
    }
    //detects if you are not touching the floor
    private void groundSensorExit(Collider2D other) {
        leftGround = true;
    }
*/
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

    private bool WallTouch(int direction) {
        Vector2 BoxDimentions = new Vector2(.1f,.1f);
        //hits walls
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        float distanceAdditon = 0.1f;
        bool wallHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.right * direction, mainCollider.size.x / 2 + distanceAdditon, layerMask);
        return wallHit;
    }
    private bool GroundTouch() {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        float distanceAdditon = 0.1f;
        //bool groundHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.down, mainCollider.size.y / 2 + distanceAdditon, layerMask);
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, mainCollider.size.y / 2 + distanceAdditon,layerMask);
        Debug.DrawRay(transform.position, Vector2.down, Color.green, .1f);
        if (groundHit) {
            jumpScript.grounded = true;
        } else {
            leftGround = true;
        }
        animatior.SetBool(NOTJUMPING, groundHit);
        return groundHit;
    }


}
