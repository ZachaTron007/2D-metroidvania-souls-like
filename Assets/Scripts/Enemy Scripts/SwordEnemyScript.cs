using Cinemachine;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordEnemyScript : EnemyScript {
    //states
    [SerializeField] private bool rightWallTouch = false;
    [SerializeField] private bool leftWallTouch = false;
    [SerializeField] private bool falling = false;
    //components
    private SpriteRenderer sr;
    //scrupts
    [SerializeField] private IdelState idelState;
    [SerializeField] private AgroState agroState;
    [SerializeField] private EnemyAttackState attackState;
    [SerializeField] private RecoveryState recoverState;
    private Health1 health;
    private State state;
    private bool idelMoving = true;
    private bool attacking = false;
    private int idelWalkSpeed = 50;
    //string literals
    private const string IDELSTOP = "IdelStop";
    private const string IDELWALK = "IdelWalkStart";
    private const string ATTACK = "Attack";
    private const string MOVING = "moving"; 
    private const string AGROWAIT = "AgroStart";
    private const string ATTACKSTART = "Attack"; 
    private const string ATTACKRESET = "AttackReset";


    private void Awake() {
        
        //scripts
        health = GetComponent<Health1>();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //listeners for the entering collisions
        //mainCollider.triggerEnter.AddListener(groundSensorEnter);

        //attacks
        //state = idelState;
        idelState.Setup(rb, animatior,playerVariable: this);
        agroState.Setup(rb, animatior, playerVariable: this);
        attackState.Setup(rb, animatior, playerVariable: this);
        recoverState.Setup(rb, animatior, playerVariable: this);
        state =idelState;
        state.Enter();

    }

    

    // Update is called once per frame
    void Update() {
        direction = WallCheck(direction);
        state.UpdateState();

        
        StateChange();
            sr.flipX = direction < 0;
    }
    private void FixedUpdate() {
        /*MOVE LEFT AND RIGHT*/
        //rb.velocity = new Vector2(direction * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        if (state.interuptable) {
            moveVetcor = move.ReadValue<Vector2>();
            StateChange();
        } else if (state.stateDone) {
            StateChange();
        }

    }

    private void StateChange() {
        State oldState = state;
        if (WithinAgroRange(direction)) {
            
            if (WithinAttackRange(direction)) {
                if (state.recover) {
                    state = attackState;
                } else {
                    state = recoverState;
                }
            } else {
                state = agroState;
            }
        } else {
            state = idelState;
        }
        
        if (oldState != state) {
            oldState.Exit();
            Debug.Log(state.name);
            state.Enter();
            
            //state.ResetState();
        }
    }

   /*
    private State AgroStart() {
        state=State.Agro;
        return state;
    }
    
    private void Attack() {
        attacking = true;
        //animatior.SetTrigger(ATTACK + attackNum);
        animatior.SetTrigger("Attack1");
        StartCoroutine(melee.Attack(direction));
        float attackRecharge = .25f;
        Invoke(ATTACKRESET, attackRecharge);

    }

    private void AttackReset() {
        attacking = false;
    }*/

}
