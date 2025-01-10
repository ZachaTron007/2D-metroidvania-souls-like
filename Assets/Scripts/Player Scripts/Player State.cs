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
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class PlayerState : Unit {
    private PlayerControls playerControls;
    public InputAction move;
    private CapsuleCollider2D clipCollider;
    //movements
    [Header("Player Variables")]
    public State xVelState;
    public State yVelState;
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
    [SerializeField] public DecelerateMoveScript decerateMoveState;
    [SerializeField] public BlockState blockState;
    [SerializeField] public BlockRecoverState blockRecoverState;
    [SerializeField] public ParryState parryState;
    [SerializeField] protected PlayerIdelState idelState;
    [SerializeField] protected GlideState glideState;
    private InputScript inputScript;
    //[SerializeField] protected PlayerAttack melee;

    //public event Action <bool> parried;
    //buttons 
    public float bufferTime = .2f;
    public float arielForce = 10f;
    float bufferCounter = 0;
    public KeyCode lastKey;
    const KeyCode jump = KeyCode.Space;
    const KeyCode dash = KeyCode.LeftShift;
    public const KeyCode glideButton = KeyCode.Space;
    const KeyCode blockButton = KeyCode.Mouse1;
    const KeyCode attackButton = KeyCode.Mouse0;
    private KeyCode[] buttons = new KeyCode[] { jump, dash , blockButton, attackButton, glideButton};


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
        decerateMoveState.Setup(rb, animatior, this);
        idelState.Setup(rb, animatior, this);
        blockState.Setup(rb, animatior, this);
        parryState.Setup(rb, animatior, this);
        blockRecoverState.Setup(rb, animatior,this);
        glideState.Setup(rb, animatior, this);
        hurtState.Setup(rb, animatior, this);
        state = idelState;

    }

    // Update is called once per frame

    void Update() {
        grounded = GroundTouch();
        lastKey = GetInput(buttons);
        //horizontal movement

        if (moveVetcor.x != 0) {
            SetDirection((int)moveVetcor.x);
            directionFlip();

        }
        //the iniatal jump, you need to be in kyote time or on the ground
        
        //if you arent on the ground
        moveVetcor = move.ReadValue<Vector2>();
        StateChange();
        yVelState?.UpdateState();
        state?.UpdateState();
    }
    private void FixedUpdate() {
        state?.FixedUpdateState();
        if (!state) {
            xVelState?.FixedUpdateState();
            yVelState?.FixedUpdateState();
        }
    }

    /*
     * Summary:
     * Handles changing of states
     * runs a selection function for each state machine
     * then checks if you can switch to that state
     */

    protected override void StateChange(State manualState = null) {
        State newXVelState = XAxisStateChange();
        State newYVelState = YAxisStateChange();
        State newActionState = ActionStateChange();
        if (!newYVelState && !newXVelState&&!newActionState) {
            newActionState = idelState;
        }
        state = CanSwitchState(newActionState,state);
        yVelState = CanSwitchState(newYVelState,yVelState);
        xVelState = CanSwitchState(newXVelState, xVelState);

    }
    /*
     * handles the state changing logic
     */
    private State XAxisStateChange() {
        if (moveVetcor.x != 0) {
            return moveState;
        } else if (Mathf.Abs(rb.linearVelocityX) > decerateMoveState.exitSpeed) {
            return decerateMoveState;
        }
        if (!xVelState) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }
        return null;
    }
    private State YAxisStateChange() {
        Debug.Log("grounded state: " + GetGroundedState());
        
        if (GetGroundedState()) {
            if (lastKey == jump) {
                return jumpScript;
            }
        }else if(lastKey == glideButton) {
            return glideState;
        } if (!yVelState && rb.linearVelocityY < 0) {
            return fallState;
        }
        if (!yVelState) {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
        }
        return null;
    }
    private State ActionStateChange() {
        dashCount += Time.deltaTime;
        if (lastKey == dash && dashCount >= dashCool) {
            dashCount = 0;
            return Dash;
        } else if (lastKey == attackButton) {
            return attackState;
        } else if (lastKey == blockButton) {
            return blockState;
        }
        return null;
    }
    /*
     * what todo when you swiutch states
     */
    protected override void SwitchStateActions(State newState,State oldState) {
        base.SwitchStateActions(newState,oldState);
        //lastKey = KeyCode.None;
    }
    /*
     * done with state logic
     */

    /*
     * handles the inputs
     * checks to see if you are pressing one of them
     * includes an input buffer to add leway
     */
    
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
            bufferCounter = 0;
        }
        return lastKey;

    }
    /*
     * what happens when you get hurt
     */
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

    public void HitSuccess() {


        /*
        Debug.Log("Buffer");
        if (!grounded) {
            rb.linearVelocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, arielForce));

        }*/
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



