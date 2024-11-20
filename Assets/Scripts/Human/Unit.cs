using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Components required by States")]
    protected Health health;
    public IncludeRBLayers includeRBLayers;
    public Rigidbody2D rb;
    public Animator animatior;
    public SpriteRenderer sr;
    public Sensors mainCollider;
    [Header("Current State")]

    public State state;
    [Header("Required Unit States")]
    //[SerializeField] protected PlayerIdelState idelState;
//    [SerializeField] protected ParentMeleeAttack melee;
    [SerializeField] protected HurtState hurtState;
    [SerializeField] protected JumpScript jumpScript;
    [SerializeField] protected FallState fallState;
    [SerializeField] protected ParentMeleeAttack attackState;
    [SerializeField] protected DeathScript dieState;
    public event Action parried;
    [Header("Properties")]
    public float attackTime;
    public bool isRecovering = false;
    [SerializeField] protected int direction = 1;//{ get; protected set; } = 1;
    protected float moveSpeed = 250;
    protected bool grounded;
    public bool engaged;
    private float kyoteTimeCounter;
    /*
     * summary:
     * get and sets
     */
    public int GetDirection() => direction;
    public virtual void SetDirection(int direction) => this.direction = direction;
    public bool GetGroundedState() => grounded;


    /*
     * summary:
     * a required function for all units to setup the nessessary components,
     * components that are special to that unit are setup in their class
     */
    protected void ComponentSetup() {
        includeRBLayers = GetComponent<IncludeRBLayers>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        //mainCollider = GetComponent<BoxCollider2D>();
        hurtState?.Setup(rb, animatior, this);
        jumpScript.Setup(rb, animatior, this);
        fallState?.Setup(rb, animatior, this);
        attackState?.Setup(rb, animatior, this);
        dieState.Setup(rb, animatior, this);
        EventSubscribe();
    }
    protected virtual void EventSubscribe() {
        health.hitEvent += GetHurt;
        health.dieEvent += EventUnsubscribe;
        health.dieEvent += Die;
    }
    protected virtual void EventUnsubscribe() {
        health.dieEvent -= EventUnsubscribe;
        health.hitEvent -= GetHurt;
    }
    /*
     * summary:
     * meant to be inherited and the logic to change the state is stored in here
     */
    protected abstract void StateChange(State manualSate = null);
    //protected abstract void GetHurt();

    /*
     * summary:
     * makes sure the you cant interupt the state,
     * and when state is done, it forces a change
     */
    protected void InteruptrableStateChange() {
        if (state.interuptable) {
            StateChange();
        } else if (state.stateDone) {
            StateChange();
        }
    }
    
    /*
     * summary:
     * sends a boxcast down to see if you are touching the ground,
     * you give a box dimentions as a parameter
     */
    protected bool GroundTouch() {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = HelperFunctions.layers["Level"];

        float distanceAdditon = 0.1f;
        RaycastHit2D groundHitLeft = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon, new Vector3(transform.position.x - mainCollider.hitBox.size.x/2, transform.position.y, 0));
        RaycastHit2D groundHitMiddle = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon,transform.position,true);
        RaycastHit2D groundHitRight = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon,new Vector3(transform.position.x + mainCollider.hitBox.size.x/2, transform.position.y, 0));
        
        if (groundHitMiddle||groundHitMiddle||groundHitRight) {
            kyoteTimeCounter = 0;
            return true;
            
        } else {
            kyoteTimeCounter += Time.deltaTime;
            if (kyoteTimeCounter > jumpScript.kyoteTime) {
                return false;
            }
            return true;
        }
    }

    protected int LayerNumToLayerMask(int layerNumber) {
        int layerMask = 1 << layerNumber;
        return layerMask;
    }
    /*
     * summary:
     * meant to be used in other functions,
     * starts a ray at the gameobject position and sends it in a direction
     */
    protected RaycastHit2D ShootRay(int direction, int layerNumber, float dist) {
        //trasnforms that number into a layer mask
        int layerMask = LayerNumToLayerMask(layerNumber);
        Vector3 startPosition = transform.position + new Vector3(transform.localScale.x / 2 * direction, transform.localScale.y / 2, 0);
        Vector2 rayDirection = new Vector2(direction, 0f);
        RaycastHit2D hit = Physics2D.Raycast(startPosition, rayDirection, dist, layerMask);
        //Debug.DrawRay(startPosition, rayDirection, Color.green, .1f);
        return hit;
    }
    public RaycastHit2D ShootRayDirection(Vector2 direction, int layerNumber, float dist, Vector3 startPosition=new Vector3(), bool debugRay = false) {
        //trasnforms that number into a layer mask
        int layerMask = LayerNumToLayerMask(layerNumber);
        //gets the height of the collider and div by 2 to get the center
        //add the offset of the collider`
        Vector3 startOffset = new Vector3(transform.localScale.x / 2 * direction.x, transform.localScale.y / 2 * direction.y, 0) + new Vector3(mainCollider.hitBox.offset.x, mainCollider.hitBox.offset.y, 0);
        if (startPosition == new Vector3()) {
            startPosition = transform.position + startOffset;
        }
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction.normalized, dist, layerMask);
        if (debugRay) {
            Debug.DrawRay(startPosition, direction.normalized, Color.green, 1f);
        }
        return hit;
    }
    public bool IsGroundInFront() {
        int layerNumber = HelperFunctions.layers["Level"]; ;
        float distanceAdditon = 0.1f;

        RaycastHit2D groundAvailible = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon, new Vector3(transform.position.x + (mainCollider.hitBox.size.x / 2) * direction, transform.position.y, 0), true);
        return groundAvailible;
    }
    /*
     * used to flip the sprite
     */
    protected void directionFlip() {
        sr.flipX = direction < 0;
    }

    protected virtual void GetHurt(bool hit,int damage) {
    }
    protected abstract void Die();
    public void HitCollided(bool hit) {
        AttackInfo attack = attackState.currentAttack;
        
    }

    protected void onParry() {
        parried?.Invoke();
    }
}
