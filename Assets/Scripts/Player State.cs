using Cinemachine;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerState : MonoBehaviour {
    private PlayerControls playerControls;
    private InputAction move;
    private BoxCollider2D mainCollider;
    private CapsuleCollider2D clipCollider;

    //movements
    [Header("Player Variables")]
    [SerializeField] private float moveSpeed = 250;
    private float tempMoveSpeed;
    public float direction { get; private set; } = 1;
    private int dashDirection = 1;

    private float dashCount;
    private float dashCool = 0.7f;

    public bool grounded { get; private set; } = true;
    //attacks
    private int attackNum;
    public float attackTime;


    //[SerializeField] private bool blood = false;

    //components
    [Header("Player Components")]
    public Animator animatior;
    public Rigidbody2D rb;
    public Vector2 moveVetcor { get; private set; }
    private SpriteRenderer sr;
    //scrupts
    [Header("States")]
    [SerializeField] private IdelState idelState;
    [SerializeField] private MeleeAttack melee;
    [SerializeField] private dashScript Dash;
    private Health1 health;
    [SerializeField] public jumpScript jumpScript;
    [SerializeField] public FallState fallState;
    [SerializeField] private wallActionsScript wallActions;
    [SerializeField] public MoveState moveState;
    [SerializeField] public BlockState blockState;

    [Header("Current State")]
    public State state;
    //buttons
    private KeyCode jump = KeyCode.Space;
    private KeyCode dash = KeyCode.LeftShift;
    public int blockButton = 1;


    private void Awake() {

        //get the input system
        playerControls = new PlayerControls();
        //scripts
        
        health = GetComponent<Health1>();
        
        //melee = attackManager.GetComponent<MeleeAttack>();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        clipCollider = GetComponent<CapsuleCollider2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        jumpScript.Setup(rb, animatior, this);
        fallState.Setup(rb, animatior, this);
        Dash.Setup(rb, animatior, this);
        moveState.Setup(rb, animatior, this);
        idelState.Setup(rb, animatior, this);
        blockState.Setup(rb, animatior, this);
        Debug.Log(melee);
        melee.Setup(rb, animatior, this);
        state = idelState;
        tempMoveSpeed = moveSpeed;

    }

    // Update is called once per frame

    void Update() {
            GroundTouch();

        //horizontal movement
        attackTime += Time.deltaTime;

        if (moveVetcor.x != 0) {
            direction = moveVetcor.x;
            sr.flipX = direction < 0;

        }
        //the iniatal jump, you need to be in kyote time or on the ground
        dashCount += Time.deltaTime;
        //if you arent on the ground
        if (state.interuptable) {
            moveVetcor = move.ReadValue<Vector2>();
            StateChange();
        } else if(state.stateDone) {
            StateChange();
        }
        state.UpdateState();
        
        

    }

    private void StateChange() {
        State oldState = state;
        if(grounded&&state!=jumpScript) {
            //checks to see if you are moving
            if (moveVetcor.x != 0) {
                state = moveState;
            } else {
                state = idelState;
                //Debug.Log("Idel State");
            }
            if (Input.GetKeyDown(jump)) {
                state = jumpScript;
            }
            //checks to see if you are falling
        }else if (rb.velocity.y < 0) {
            state = fallState;
        }
        if (Input.GetKeyDown(dash) && dashCount >= dashCool && !Dash.dashing) {
            state = Dash;
            dashCount = 0;
        }
        attackTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0)) {
            state = melee;
            // Reset timer
            //attackTime = 0.0f;
        }
        if (Input.GetMouseButtonDown(blockButton)) {
            state=blockState;
        }

        //Debug.Log("is "+oldState.name+" interuptable: "+oldState.interuptable);
        if(oldState!= state) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            state.ResetState();
        }
    }
    private void FixedUpdate() {
        state.FixedUpdateState();/*
        if (state.interuptable) {
            moveSpeed = tempMoveSpeed;
            Run();
        } else {
            moveSpeed = 0.0f;
        }
        if (!state.interuptable && rb.velocity.x == moveVetcor.x * moveSpeed * Time.fixedDeltaTime) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        } else if(state.interuptable) {
            Run();
        }*/
        if(state.interuptable) {
            Run();
        }
    }

    
    private void Run() {
        if (rb.velocity.x == 0) {
            rb.velocity = new Vector2(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(moveVetcor.x * Mathf.Abs(rb.velocity.x), rb.velocity.y);
        }
        
    }

    private bool GroundTouch() {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        float distanceAdditon = 0.1f;
        //bool groundHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.down, mainCollider.size.y / 2 + distanceAdditon, layerMask);
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down,distanceAdditon, layerMask);
        Debug.DrawRay(transform.position, Vector2.down, Color.green, .1f);
        if (groundHit) {
            grounded = true;
        } else {
            grounded = false;
        }
        //animatior.SetBool(NOTJUMPING, groundHit);
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
