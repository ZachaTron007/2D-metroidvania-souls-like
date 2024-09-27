using Cinemachine;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Temp : MonoBehaviour {
    private PlayerControls playerControls;
    private InputAction move;
    private BoxCollider2D mainCollider;
    private CapsuleCollider2D clipCollider;

    //movements
    private float moveSpeed = 250;
    [SerializeField] public float direction = 1;
    private int dashDirection = 1;

    private float dashCount;
    private float dashCool = 0.7f;

    public bool stateDone = true;

    public bool grounded = true;


    //[SerializeField] private bool blood = false;

    //components
    public Animator animatior;
    public Rigidbody2D rb;
    public Vector2 moveVetcor;
    private SpriteRenderer sr;
    //scrupts
    [SerializeField] private IdelState idelState;
    [SerializeField] private GameObject attackManager;
    [SerializeField] private MeleeAttack melee;
    [SerializeField] private dashScript Dash;
    private Health1 health;
    [SerializeField] public jumpScript jumpScript;
    [SerializeField] private wallActionsScript wallActions;
    [SerializeField] public MoveState moveState;
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

    [SerializeField] private State state;

    private void Awake() {

        //get the input system
        playerControls = new PlayerControls();
        //scripts
        
        health = GetComponent<Health1>();
        
        melee = attackManager.GetComponent<MeleeAttack>();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        clipCollider = GetComponent<CapsuleCollider2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //Dash  = new dashScript();

        jumpScript.Setup(rb, animatior, this);
        Dash.Setup(rb, animatior, this);
        moveState.Setup(rb, animatior, this);

    }

    // Update is called once per frame

    void Update() {
        GroundTouch();
        //horizontal movement
        //Debug.Log(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        /*
        if (!Dash.dashing) {
            moveVetcor = move.ReadValue<Vector2>();

        }*/

        if (moveVetcor.x != 0) {
            direction = moveVetcor.x;
            sr.flipX = direction < 0;

        }
        //the iniatal jump, you need to be in kyote time or on the ground
        dashCount += Time.deltaTime;
        /*DASH ACTIVATE*/
        //Debug.Log(state);
        //if you arent on the ground
        //if (stateDone) {
            StateChange();
        //}
        moveVetcor = move.ReadValue<Vector2>();
        //Debug.Log(stateDone);

    }

    private void StateChange() {
        State oldState = state;
        if (moveVetcor.x != 0&&state!=moveState) {
            state = moveState;
            Debug.Log("Move State");
        }
        if (grounded) {
            if (Input.GetKeyDown(jump)) {
                state = jumpScript;
                Debug.Log("Jump State");
            }
        }
        if (Input.GetKeyDown(dash) && dashCount >= dashCool && !Dash.dashing) {
            state = Dash;
            dashCount = 0;
            Debug.Log("Dash State");
        }
        if (!state) {
            state = idelState;
            Debug.Log("Idel State");
        }
        //Debug.Log(oldState.stateDone);
        if(oldState!= state){//&&oldState.stateDone) {
            Debug.Log("State Change");
            state.ResetState();
            state.Enter();
        }
    }
    private void FixedUpdate() {
        if(state)
        state.FixedUpdateState();
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
    *//*
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Spike") {
            Debug.Log("hit");
            health.Damage(1);
        }
    }
    

    private bool WallTouch(int direction) {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        float distanceAdditon = 0.1f;
        bool wallHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.right * direction, mainCollider.size.x / 2 + distanceAdditon, layerMask);
        return wallHit;
    }
        */
    private bool GroundTouch() {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        float distanceAdditon = 0.1f;
        //bool groundHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.down, mainCollider.size.y / 2 + distanceAdditon, layerMask);
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, mainCollider.size.y / 2 + distanceAdditon, layerMask);
        Debug.DrawRay(transform.position, Vector2.down, Color.green, .1f);
        if (groundHit) {
            grounded = true;
        } else {
            grounded = false;
        }
        animatior.SetBool(NOTJUMPING, groundHit);
        return groundHit;
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
