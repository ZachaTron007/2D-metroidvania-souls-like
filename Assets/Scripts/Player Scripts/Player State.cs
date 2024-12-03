using Cinemachine;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerState : Unit {
    private PlayerControls playerControls;
    private InputAction move;
    private CapsuleCollider2D clipCollider;

    //movements
    [Header("Player Variables")]
    //public float direction { get; private set; } = 1;

    private float dashCount;
    private readonly float dashCool = 0.7f;
    private bool canParry;


    //[SerializeField] private bool blood = false;

    //components
    //[Header("Player Components")]
    public Vector2 moveVetcor { get; private set; }
    //scrupts
    [Header("States")]
    [SerializeField] private dashScript Dash;
    [SerializeField] private wallActionsScript wallActions;
    [SerializeField] public MoveState moveState;
    [SerializeField] public BlockState blockState;
    [SerializeField] public BlockRecoverState blockRecoverState;
    [SerializeField] public ParryState parryState;
    [SerializeField] protected PlayerIdelState idelState;
    private InputScript inputScript;
    //[SerializeField] protected PlayerAttack melee;

    //public event Action <bool> parried;
    //buttons 
    public KeyCode lastKey;
    private KeyCode jump = KeyCode.Space;
    private KeyCode dash = KeyCode.LeftShift;
    private KeyCode blockButton = KeyCode.Mouse1;
    private KeyCode attackButton = KeyCode.Mouse0;


    protected override void EventSubscribe() {
        base.EventSubscribe();
    }
    protected override void EventUnsubscribe() {
        base.EventUnsubscribe();
        playerControls.Player.Disable();
    }

    private void Awake() {
        //get the input system
        playerControls = new PlayerControls();
        inputScript = GetComponent<InputScript>();

        //melee = attackManager.GetComponent<MeleeAttack>();
        //get the rigidbody and collider reffrences
        ComponentSetup();
        move = playerControls.Player.Move;
        move.Enable();

        jumpScript.Setup(rb, animatior, this);
        fallState.Setup(rb, animatior, this);
        Dash.Setup(rb, animatior, this);
        moveState.Setup(rb, animatior, this);
        idelState.Setup(rb, animatior, this);
        blockState.Setup(rb, animatior, this);
        parryState.Setup(rb, animatior, this);
        blockRecoverState.Setup(rb, animatior,this);
        //melee.Setup(rb, animatior, this);
        hurtState.Setup(rb, animatior, this);
        state = idelState;

    }

    // Update is called once per frame

    void Update() {
        grounded = GroundTouch();
        lastKey = GetInput();
        //horizontal movement
        attackTime += Time.deltaTime;

        if (moveVetcor.x != 0) {
            SetDirection((int)moveVetcor.x);
            directionFlip();

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



    protected override void StateChange(State manualState = null) {
        State oldState = state;
        if (grounded && state != jumpScript) {
            //checks to see if you are moving
            if (moveVetcor.x != 0) {
                state = moveState;
            } else {
                state = idelState;
            }
            if (lastKey == jump) {
                state = jumpScript;
            }

            //checks to see if you are falling
        } else if (rb.linearVelocity.y < 0) {
            state = fallState;
        }
        if (lastKey == dash && dashCount >= dashCool) {
            state = Dash;
            dashCount = 0;
        }
        attackTime += Time.deltaTime;
        if (grounded) {
            if (lastKey == attackButton && attackTime >= attackState.currentAttack.length) {
                state = attackState;
            }
            if (lastKey == blockButton) {
                state = blockState;
            }
        }
        if (manualState) {
            state = manualState;
        }
        if(oldState!= state) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            state.ResetState(oldState);
            lastKey = KeyCode.Mouse6;
        }
    }
    private void FixedUpdate() {
        state.FixedUpdateState();
        if(state.interuptable) {
            Run();
        }
    }

    
    private void Run() {
        if (rb.linearVelocity.x == 0) {
            rb.linearVelocity = new Vector2(moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
        } else {
            rb.linearVelocity = new Vector2(moveVetcor.x * Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y);
        }
        
    }
    public KeyCode GetInput() {
        if (Input.GetKeyDown(jump)) {
            lastKey = jump;
        } else if (Input.GetKeyDown(dash)) {
            lastKey = dash;
        } else if (Input.GetKeyDown(attackButton)) {
            lastKey = attackButton;
        } else if (Input.GetKeyDown(blockButton)) {
            lastKey = blockButton;
        }
        return lastKey;

    }

    protected override void GetHurt(bool hit, DamageScript EnemyAttack) {
        base.GetHurt(hit, EnemyAttack);
        if (!hit&&blockState.canParry) {
            StateChange(parryState);
            onParry();
        }else if (!hit) {
            StateChange(blockRecoverState);
        }
        if(hit) {
            StateChange(hurtState);
        }
    }

    protected override void Die() {
        StateChange(dieState);
    }

    public void StateChanges() {
        StateChange(blockRecoverState);
    }


    private void OnEnable() {
        //move = playerControls.Player.Move;
        //move.Enable();
    }
    private void OnDisable() {
        //move = playerControls.Player.Move;
        //move.Disable();
    }
    //protected override void GetHurt() {
    //    StateChange(hurtState);
    //}

}



