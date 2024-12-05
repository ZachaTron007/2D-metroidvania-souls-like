using Cinemachine;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
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
    private float bufferTime = .2f;
    private float bufferCounter = 0;
    public KeyCode lastKey;
    const KeyCode jump = KeyCode.Space;
    const KeyCode dash = KeyCode.LeftShift;
    const KeyCode blockButton = KeyCode.Mouse1;
    const KeyCode attackButton = KeyCode.Mouse0;
    private KeyCode[] buttons = new KeyCode[] { jump, dash , blockButton, attackButton };


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
        lastKey = GetInput(buttons);
        //horizontal movement
        attackTime += Time.deltaTime;

        if (moveVetcor.x != 0) {
            SetDirection((int)moveVetcor.x);
            directionFlip();

        }
        //the iniatal jump, you need to be in kyote time or on the ground
        dashCount += Time.deltaTime;
        //if you arent on the ground
        moveVetcor = move.ReadValue<Vector2>();
        StateChange();
        state.UpdateState();
    }



    protected override void StateChange(State manualState = null) {
        State newState = state;

        if (grounded && state != jumpScript) {
            //checks to see if you are moving
            if (moveVetcor.x != 0) {
                newState = moveState;
            } else {
                newState = idelState;
            }
            if (lastKey == jump) {
                newState = jumpScript;
            }

            //checks to see if you are falling
        } else if (rb.linearVelocity.y < 0) {
            newState = fallState;
        }
        if (lastKey == dash && dashCount >= dashCool) {
            newState = Dash;
            dashCount = 0;
        }
        attackTime += Time.deltaTime;
        if (grounded) {
            if (lastKey == attackButton && attackTime >= attackState.currentAttack.length) {
                newState = attackState;
            }
            if (lastKey == blockButton) {
                newState = blockState;
            }
        }
        if (manualState) {
            newState = manualState;
        }
        state = CanSwitchState(newState);
    }
    protected override void SwitchStateActions() {
        base.SwitchStateActions();
        lastKey = KeyCode.None;
    }

    private void FixedUpdate() {
        state.FixedUpdateState();
        if(state.interuptable==0) {
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
    public KeyCode GetInput(KeyCode[]buttons) {
        
        for (int i = 0; i < buttons.Length; i++){
            if (Input.GetKeyDown(buttons[i])){
                lastKey = buttons[i];
                bufferCounter = 0;
            }
        }
        bufferCounter += Time.deltaTime;
        if (bufferCounter >= bufferTime) {
            lastKey = KeyCode.None;
        } else {
            bufferCounter = 0;
        }
        return lastKey;

    }

    protected override void GetHurt(bool hit, DamageScript EnemyAttack) {
        base.GetHurt(hit, EnemyAttack);
        if (!hit) {
            if (blockState.canParry) {
                StateChange(parryState);
                onParry();
            } else {
                StateChange(blockRecoverState);
            }
        } else{
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



