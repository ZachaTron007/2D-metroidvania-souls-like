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
    [SerializeField] private MeleeAttack melee;
    [SerializeField] private IdelState idelState;
    [SerializeField] private AgroState agroState;
    [SerializeField] private AttackState attackState;
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

    }

    

    // Update is called once per frame
    void Update() {

        /*
        switch (state) {
            case State.Idel:
                if (moveSpeed > idelWalkSpeed) {
                    moveSpeed = idelWalkSpeed;
                    idelMoving = true;
                }
                direction = WallCheck(direction);
                //float waitTime = .4f;
                state = WithinAgroRange(direction) ? State.Agro : State.Idel;
                if (idelMoving) {
                    float stopLowSpeed = 1f;
                    float stopHighSpeed = 2f;
                    Invoke(IDELSTOP, UnityEngine.Random.Range(stopLowSpeed, stopHighSpeed));
                    idelMoving = false;
                }
                break;

            case State.Agro:
                if (!WithinAgroRange(direction)) {
                    state = State.Idel;
                    break;
                }
                state = WithinAttackRange(direction) ? State.Attack : State.Agro;
                int agroSpeed = 150;
                moveSpeed = agroSpeed;
                break;

            case State.Attack:
                moveSpeed = 0;
                float attackRecharge = 1f;
                if (!attacking) {
                    if (!WithinAttackRange(direction)) {
                        state = State.Idel;
                        break;
                    }
                    Invoke(ATTACKSTART, attackRecharge);
                    attacking = true;
                }
                break;
            default:
                state = State.Idel;
                break;
        }*/

        //horizontal movement
        StateChange();
            sr.flipX = direction < 0;
    }
    private void FixedUpdate() {
        /*MOVE LEFT AND RIGHT*/
        //rb.velocity = new Vector2(direction * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);

    }

    private void StateChange() {
        State oldState = state;
        if (WithinAgroRange(direction)) {
            if (WithinAttackRange(direction)) {
                state = attackState;
            } else {
                state = agroState;
            }
        } else {
            state = idelState;
        }
        if (oldState != state) {
            state.Enter();
            //oldState.Exit();
            //state.ResetState();
        }
    }

    /*
    private void IdelStop() {
        if (state == State.Idel) {

            float stopLowSpeed = 1f;
            float stopHighSpeed = 2f;
            Invoke(IDELWALK,UnityEngine.Random.Range(stopLowSpeed, stopHighSpeed));
            int stayStillSpeed = 0;
            moveSpeed = stayStillSpeed;
        }
        
    }
    private void IdelWalkStart() {
        if (state == State.Idel) {

            //direction *= -1;
            
            moveSpeed = idelWalkSpeed;
            idelMoving = true;
        }
    }
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
