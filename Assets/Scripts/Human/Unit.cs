using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Components required by States")]
    protected Health health;
    public Rigidbody2D rb;
    public Animator animatior;
    protected SpriteRenderer sr;
    protected BoxCollider2D mainCollider;
    [Header("Current State")]

    [SerializeField] protected State state;

    [Header("Properties")]
    public float attackTime;
    public int direction { get; protected set; } = 1;
    public float moveSpeed { get; protected set; } = 250;
    public bool grounded { get; protected set; }
    
    /*
     * summary:
     * a required function for all units to setup the nessessary components,
     * components that are special to that unit are setup in their class
     */
    public void ComponentSetup() {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        animatior = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        mainCollider = GetComponent<BoxCollider2D>();
    }

    /*
     * summary:
     * meant to be inherited and the logic to change the state is stored in here
     */
    protected abstract void StateChange();

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
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        float distanceAdditon = 0.1f;
        //bool groundHit = Physics2D.BoxCast(transform.position, BoxDimentions, 0, Vector2.down, mainCollider.size.y / 2 + distanceAdditon, layerMask);
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, distanceAdditon, layerMask);
        Debug.DrawRay(transform.position, Vector2.down, Color.green, .1f);
        if (groundHit) {
            grounded = true;
        } else {
            grounded = false;
        }
        //animatior.SetBool(NOTJUMPING, groundHit);
        return groundHit;
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
        Vector3 startPosition = gameObject.transform.position + new Vector3(transform.localScale.x / 2 * direction, gameObject.transform.localScale.y / 2, 0);
        Vector2 rayDirection = new Vector2(direction, 0f);
        RaycastHit2D hit = Physics2D.Raycast(startPosition, rayDirection, dist, layerMask);
        Debug.DrawRay(startPosition, rayDirection, Color.green, .1f);
        return hit;
    }
    protected RaycastHit2D ShootRayDirection(Vector2 direction, int layerNumber, float dist) {
        //trasnforms that number into a layer mask
        int layerMask = LayerNumToLayerMask(layerNumber);
        Vector3 startPosition = gameObject.transform.position + new Vector3(transform.localScale.x / 2 * direction.x, gameObject.transform.localScale.y / 2 * direction.y, 0);
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, dist, layerMask);
        Debug.DrawRay(startPosition, direction, Color.green, .1f);
        return hit;
    }
    /*
     * used to flip the sprite
     */
    protected void directionFlip() {
        sr.flipX = direction < 0;
    }
}
