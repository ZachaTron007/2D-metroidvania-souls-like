using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class Unit : MonoBehaviour
{
    [Header("Components required by States")]
    public AnimationCollection animations;
    protected Health health;
    protected Stun stun;
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
    [HideInInspector] public AttackInfo lastAttackToHit;
    public event Action parried;
    [Header("Properties")]
    [HideInInspector] public bool canBeHit = true;
    [HideInInspector] public bool isRecovering = false;
    [SerializeField] private int direction = 1;//{ get; protected set; } = 1;
    protected bool grounded;
    private float kyoteTimeCounter;
    /*
     * summary:
     * get and sets
     */
    public int GetDirection() => direction;
    public virtual void SetDirection(int direction) {
        this.direction = direction;
        sr.flipX = direction < 0;
    }
    [ContextMenu("Flip Directions")]
    public void FlipDirection() {
        SetDirection(GetDirection()*-1);
    }
    public bool GetGroundedState() => grounded;


    /*
     * summary:
     * a required function for all units to setup the nessessary components,
     * components that are special to that unit are setup in their class
     */
    protected virtual void Update() {
        grounded = GroundTouch();
    }
    protected void ComponentSetup() {
        state = fallState;
        includeRBLayers = GetComponent<IncludeRBLayers>();
        health = GetComponent<Health>();
        TryGetComponent(out stun);

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
     * sends a boxcast down to see if you are touching the ground,
     * you give a box dimentions as a parameter
     */

    protected State CanSwitchState(State newState, State oldState) {
        if (!oldState) {
            SwitchStateActions(newState, oldState);
            return newState;
        } else if (!newState) {
            if (oldState.IsStateDone()||oldState.interuptable==0) {
                return null;
            } else {
                //SwitchStateActions(newState, oldState);
                return oldState;
            }
        } else {
        }
        if (newState.interuptable >= oldState.interuptable || oldState.IsStateDone() || oldState.interuptable == 0) {
            if (oldState != newState) {
                SwitchStateActions(newState, oldState);

                return newState;
            } else if (oldState.IsStateDone() && oldState.canTransitionToSelf) {
                SwitchStateActions(newState,oldState);
                return newState;
            }
        }

        return oldState;
    }
    protected virtual void SwitchStateActions(State newState,State oldState) {
        if (oldState) {
            oldState.ResetState(newState);
        } else if(newState) {
            newState.ResetState(newState);
        }
    }
    protected bool GroundTouch() {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = HelperFunctions.layers["Level"];

        float distanceAdditon = -.4f+mainCollider.hitBox.size.y/2;
        RaycastHit2D groundHitLeft = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon, new Vector3(transform.position.x - mainCollider.hitBox.size.x/2, transform.position.y + .2f, 0));
        RaycastHit2D groundHitMiddle = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon,new Vector3(transform.position.x, transform.position.y + .2f, 0),true);
        RaycastHit2D groundHitRight = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon,new Vector3(transform.position.x + mainCollider.hitBox.size.x/2, transform.position.y + .2f, 0));
        
        if (groundHitMiddle||groundHitMiddle||groundHitRight) {
            kyoteTimeCounter = 0;
            CrumblingPlatformScript platformScript;
            if (groundHitLeft) {
                groundHitLeft.gameObject.TryGetComponent(out platformScript);
            } else if(groundHitMiddle) {
                groundHitRight.gameObject.TryGetComponent(out platformScript);
            } else if (groundHitRight) {
                groundHitRight.gameObject.TryGetComponent(out platformScript);
            }
            //platformScript?.
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

        RaycastHit2D groundAvailible = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon*2, new Vector3(transform.position.x + (mainCollider.hitBox.size.x / 2) * direction, transform.position.y+distanceAdditon, 0), true);
        return groundAvailible;
    }
    /*
     * used to flip the sprite
     */
    protected void directionFlip() {
        
    }

    protected virtual void GetHurt(bool hit,DamageScript enemyAttack) {
    }
    protected abstract void Die();
    public void HitCollided(bool hit) {
        AttackInfo attack = attackState.currentAttack;
        
    }

    protected void onParry() {
        parried?.Invoke();
    }
}
