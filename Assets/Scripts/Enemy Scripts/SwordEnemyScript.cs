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
    [SerializeField] private Sensors groundSensor;
    //movements
    [SerializeField] private float moveSpeed = 250;
    private int direction = 1;
    //states
    [SerializeField] private bool rightWallTouch = false;
    [SerializeField] private bool leftWallTouch = false;
    [SerializeField] private bool falling = false;
    //components
    Animator animatior;
    private SpriteRenderer sr;
    //scrupts
    [SerializeField] private GameObject attackManager;
    private MeleeAttack melee;
    private Health1 health;
    private jumpScript jumpScript;
    private State state = State.Idel;
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
        jumpScript = GetComponent<jumpScript>();
        melee = attackManager.GetComponent<MeleeAttack>();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //listeners for the entering collisions
        //mainCollider.triggerEnter.AddListener(groundSensorEnter);
        groundSensor.triggerEnter.AddListener(groundSensorEnter);
        //listeners for the exiting collisions
        groundSensor.triggerExit.AddListener(groundSensorExit);
        //attacks


    }

    

    // Update is called once per frame
    void Update() {
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
        }

        //horizontal movement
        /*GETS DIRECTION*/
        if (rb.velocity.y != 0) {
            falling = rb.velocity.y < 0;

        } else {
            falling = false;

        }
            sr.flipX = direction < 0;

        /*SETS RUN ANIMATION VAR*/
        animatior.SetFloat(MOVING, moveSpeed);
    }
    private void FixedUpdate() {
        /*MOVE LEFT AND RIGHT*/
        rb.velocity = new Vector2(direction * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);

    }

    /*COLLISIONS AND ENABLE AND DISABLE*/
    private void groundSensorEnter(Collider2D other) {
        jumpScript.grounded = true;
    }
    //detects if you are not touching the floor
    private void groundSensorExit(Collider2D other) {
        //leftGround = true;
    }

   
  

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
        melee.lookDirection.x = direction;
        melee.Attack();
        float attackRecharge = .25f;
        Invoke(ATTACKRESET, attackRecharge);

    }

    private void AttackReset() {
        attacking = false;
    }

}
