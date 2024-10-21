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

public class PlayerState : Unit {
    private PlayerControls playerControls;
    private InputAction move;
    private CapsuleCollider2D clipCollider;

    //movements
    [Header("Player Variables")]
    //public float direction { get; private set; } = 1;

    private float dashCount;
    private float dashCool = 0.7f;


    //[SerializeField] private bool blood = false;

    //components
    //[Header("Player Components")]
    public Vector2 moveVetcor { get; private set; }
    //scrupts
    [Header("States")]
    [SerializeField] private dashScript Dash;
    [SerializeField] public jumpScript jumpScript;
    [SerializeField] private wallActionsScript wallActions;
    [SerializeField] public MoveState moveState;
    [SerializeField] public BlockState blockState;
    [SerializeField] protected PlayerIdelState idelState;
    [SerializeField] protected PlayerAttack melee;
    

    //buttons
    private KeyCode jump = KeyCode.Space;
    private KeyCode dash = KeyCode.LeftShift;
    public int blockButton = 1;


    private void Awake() {
        //CinemachineEffectScript.instance.ScreenShake(0.1f, 0.1f);
        //get the input system
        playerControls = new PlayerControls();

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
        melee.Setup(rb, animatior, this);
        hurtState.Setup(rb, animatior, this);
        state = idelState;
        

    }

    // Update is called once per frame

    void Update() {
        GroundTouch();

        //horizontal movement
        attackTime += Time.deltaTime;

        if (moveVetcor.x != 0) {
            direction = (int)moveVetcor.x;
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

    protected override void StateChange(State manualState=null) {
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
        }else if (rb.linearVelocity.y < 0) {
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
        if(manualState)
            state = manualState;
        if(oldState!= state) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            state.ResetState(oldState);
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

    private void OnEnable() {
        //move = playerControls.Player.Move;
        //move.Enable();
    }
    private void OnDisable() {
        //move = playerControls.Player.Move;
        //move.Disable();
    }
    
}
