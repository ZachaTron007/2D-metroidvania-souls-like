using NUnit.Framework.Internal;
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
    public LinkedList<AnimationClip> animationList = new LinkedList<AnimationClip> { };
    protected Health health;
    protected Stun stun;
    public IncludeRBLayers includeRBLayers;
    public Rigidbody2D rb;
    public Animator animatior;
    public SpriteRenderer sr;
    public Sensors mainCollider;
    [Header("Current State")]
    public State state;
    
    public event Action parried;
    [Header("Properties")]
    [HideInInspector] public bool canBeHit = true;
    [HideInInspector] public bool isRecovering = false;
    [SerializeField] private int direction = 1;//{ get; protected set; } = 1;
    public Vector2 forceAdded = new Vector2(0, 0);
    protected bool grounded;
    private float kyoteTimeCounter;
    private CustomPlatformBase lastPlat = null;
    [Header("Required Unit States")]
    //[SerializeField] protected PlayerIdelState idelState;
    //    [SerializeField] protected ParentMeleeAttack melee;
    [SerializeField] protected HurtState hurtState;
    [SerializeField] protected JumpScript jumpScript;
    [SerializeField] protected FallState fallState;
    [SerializeField] protected DeathScript dieState;
    [HideInInspector] public AttackInfo lastAttackToHit;
    public ParentMeleeAttack currentAttack;
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
        jumpScript?.Setup(rb, animatior, this);
        fallState?.Setup(rb, animatior, this);
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
    public abstract void StateChange(State manualSate = null);
    //protected abstract void GetHurt();

    protected State CanSwitchState(State newState, State oldState) {
        //if you dont have an old state, automaticly siwtch to newstate
        //Debug.Log("OldState: " + state+"New State: "+newState);
        if (!oldState) {
            SwitchStateActions(newState, oldState);
            return newState;
        } else if (!newState) {
            //if (oldState.name == "Attack Hitbox") Debug.Log("Box");
            //if you arnt switching to a new state
            //if you are done with the old state then exit
            if (oldState.IsStateDone()||oldState.interuptable==0) {
                SwitchStateActions(newState, oldState);
                return null;
            }
            //if not continue
            return oldState;
        }
        //if (oldState.name == "Attack Hitbox") Debug.Log("Box");
        //if you can switch states
        if (newState.interuptable >= oldState.interuptable || oldState.IsStateDone() || oldState.interuptable == 0) {
            //if the states are diffrent
            if (oldState != newState) {
                SwitchStateActions(newState, oldState);
                return newState;


            } else if (oldState.IsStateDone() && oldState.canInteruptSelf) {
                //if they are the same check if it is done, or if you can switch to the same state
                SwitchStateActions(newState,oldState);
                return newState;
            }
        }

        return oldState;
    }
    protected virtual void SwitchStateActions(State newState,State oldState) {
        //Debug.Log("Calling Exit on" + oldState);
        if (oldState) {
            //Debug.Log("should call exit on: "+oldState);
            oldState.ResetState(newState);
        } else if(newState) {
            newState.ResetState(newState,false);
        }
    }
    private void PlatformScriptLogic(CustomPlatformBase scriptCollected) {
        //if you are on a platform you wernt on last frame
        if (scriptCollected != lastPlat&&scriptCollected) {
            scriptCollected.EnterOnCustomPlatform(rb);
            
        }
        scriptCollected?.StayOnCustomPlatform(rb);
        //if you left a platform this frame
        if (lastPlat && !scriptCollected) {
            lastPlat.ExitOnCustomPlatform(rb);
        }
        lastPlat = scriptCollected;
    }
    protected bool GroundTouch() {
        Vector2 BoxDimentions = new Vector2(.1f, .1f);
        //hits walls
        int layerNumber = HelperFunctions.layers["Level"];

        float distanceAdditon = -.4f+mainCollider.hitBox.size.y/2;
        RaycastHit2D[] rays = new RaycastHit2D[3];
        CustomPlatformBase platformScript = null;
        bool grounded = false;
        for(int i = 0; i < rays.Length; i++) {
            rays[i] = ShootRayDirection(Vector2.down, layerNumber, distanceAdditon, new Vector3(transform.position.x - (i-1)*(mainCollider.hitBox.size.x / 2), transform.position.y + .2f, 0));
            if (rays[i]) {
                rays[i].collider.gameObject.TryGetComponent(out platformScript);
                grounded = true; 
                break;
            }
        }
        PlatformScriptLogic(platformScript);
        if (grounded) {
            kyoteTimeCounter = 0;
            return true;
        }
        kyoteTimeCounter += Time.deltaTime;
        if (kyoteTimeCounter > jumpScript.kyoteTime) {
            return false;
        }
        PlatformScriptLogic(platformScript);
        return true;

    }

    protected bool WallCheck(float maxDist) {
        //layers to hit
        RaycastHit2D hit = ShootRayDirection(GetDirection()*Vector2.right, HelperFunctions.layers["Level"], maxDist+mainCollider.hitBox.size.x/2, new Vector3(transform.position.x + (mainCollider.hitBox.offset.x * (mainCollider.hitBox.size.x/2-.2f)), transform.position.y+(mainCollider.hitBox.offset.y*(mainCollider.hitBox.size.y/2)),0));
        if (hit) {
            return true;
        }

        return false;
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

    public void SpawnEffect(GameObject effect, Vector2 startPos, Quaternion rotation, float destroyDelay, Vector2 offset = new Vector2(), float effectSpeed = 1) {
        if(offset == new Vector2())     offset = Vector2.zero;
        effect = Instantiate(effect, startPos + offset, rotation);
        effect.GetComponent<Animator>().speed = effectSpeed;
        Destroy(effect, destroyDelay);
    }

    /*
     * used to flip the sprite
     */
    protected void directionFlip() {
        
    }

    protected virtual void GetHurt(bool hit,DamageScript enemyAttack) {

    }
    protected abstract void Die();

    public virtual void HitSucsess() {

    }

    protected void onParry() {
        parried?.Invoke();
    }
}
