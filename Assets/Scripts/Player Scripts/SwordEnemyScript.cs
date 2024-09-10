using Cinemachine;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordEnemyScript : EnemyScript {
    private BoxCollider2D mainCollider;
    [SerializeField] private Sensors groundSensor;
    //movements
    [SerializeField] private float moveSpeed = 250;
    private int direction = 1;
    //attacks
    private int attackNum;
    private float attackTime;
    //states
    [SerializeField] private bool rightWallTouch = false;
    [SerializeField] private bool leftWallTouch = false;
    [SerializeField] private bool falling = false;
    //components
    Animator animatior;
    //public Rigidbody2D rb;
    private Vector2 moveVetcor;
    private SpriteRenderer sr;
    //scrupts
    [SerializeField] private GameObject attackManager;
    private MeleeAttack melee;
    private Health1 health;
    private jumpScript jumpScript;

    private void Awake() {
        //scripts
        health = GetComponent<Health1>();
        jumpScript = GetComponent<jumpScript>();
        melee = attackManager.GetComponent<MeleeAttack>();
        //get the rigidbody and collider reffrences
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
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
        direction=wallCheck(direction);
        moveVetcor.x = 1;
        //horizontal movement
        /*GETS DIRECTION*/
        if (rb.velocity.y != 0) {
            falling = rb.velocity.y < 0;

        } else {
            falling = false;

        }
            sr.flipX = direction < 0;


        if (Input.GetMouseButtonDown(0) && attackTime > 0.25f) {
            attackNum++;
            // Loop back to one after third attack
            if (attackNum > 3)
                attackNum = 1;

            // Reset Attack combo if time since last attack is too large
            if (attackTime > 1.0f) {
                attackNum = 1;
            }
            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animatior.SetTrigger("Attack" + attackNum);
            melee.lookDirection.x = direction;
            melee.Attack();

            // Reset timer
            attackTime = 0.0f;
        }
        attackTime += Time.deltaTime;
        /*SETS RUN ANIMATION VAR*/
        animatior.SetFloat("moving", Mathf.Abs(moveVetcor.x));
    }
    private void FixedUpdate() {

        //actualy moves you left and right using physics
        //if (!wallActions.wallJump && !Dash.dashing) {
            /*MOVE LEFT AND RIGHT*/
        rb.velocity = new Vector2(direction * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        //Debug.Log(direction * moveSpeed * Time.fixedDeltaTime);
        
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
    /*COLLISIONS AND ENABLE AND DISABLE*/
    private void groundSensorEnter(Collider2D other) {
        jumpScript.grounded = true;
    }
    //detects if you are not touching the floor
    private void groundSensorExit(Collider2D other) {
        //leftGround = true;
    }

   
   

    private bool WallTouch(int direction) {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = 6;
        int layerMask = 1<<layerNumber;
        float distanceAdditon = 0.1f;
        bool wallHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.right * direction, mainCollider.size.x / 2 + distanceAdditon, layerMask);
        return wallHit;
    }


}
