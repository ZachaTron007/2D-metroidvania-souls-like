using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EnemyScript : Unit {
    protected float maxDist = .3f;
    [Header("Awareness Colliders")]
    [SerializeField] protected GameObject agroRange;
    [SerializeField] protected GameObject attackRange;
    protected BoxCollider2D agroRangeHitbox;
    protected BoxCollider2D attackRangeHitbox;
    protected Sensors agroRangeScript;
    protected Sensors attackRangeScript;
    protected bool isWithinAgroRange = false;
    protected bool isWithinAttackRange = false;

    protected int WallCheck() {
        //layers to hit
        int layerNumber = 6;
        RaycastHit2D hit = ShootRay(direction, layerNumber, maxDist);
        if (hit) {
            direction *= -1;
        }
        
        return direction;
    }
    
    /*
     * summary:
     * collider enter and exit events for agro and attack range
     */
    protected void AgroRangeEnter(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            isWithinAgroRange = true;
            
        }
    }
    protected void AgroRangeEnter(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Agro");
            isWithinAgroRange = true;

        }
    }
    protected void AgroRangeStay(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Vector2 directionOfPlayer = other.gameObject.transform.position - gameObject.transform.position;
            if (!state.interuptable) {
                int tempDirection = direction;
                direction = directionOfPlayer.x > direction ? 1 : -1;
                if (tempDirection != direction) {
                    agroRangeHitbox.offset *= new Vector2(-1, 1);
                    attackRangeHitbox.offset *= new Vector2(-1, 1);
                }
            }
            if (directionOfPlayer.y>0) {
                if(ShootRayDirection(Vector2.up, 6, directionOfPlayer.y)) {
                    isWithinAgroRange = false;
                } else {
                    isWithinAgroRange = true;
                }
            }
        }
    }
    protected void AgroRangeExit(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            isWithinAgroRange = false;
        }
    }
    
    protected void AttackRangeEnter(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            
            isWithinAttackRange = true;
        }
    }
    protected void AttackRangeStay(Collider2D other) {
        
    }
    protected void AttackRangeExit(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            isWithinAttackRange = false;
        }
    }
    
    /*
     * summary:
     * subscribing to the events
     * runs when the object is created
     */
    protected void AgroAttackColliders() {
        attackRangeScript = attackRange.GetComponent<Sensors>();
        agroRangeScript = agroRange.GetComponent<Sensors>();
        agroRangeHitbox = agroRange.GetComponent<BoxCollider2D>();
        attackRangeHitbox = attackRange.GetComponent<BoxCollider2D>();

        agroRangeScript.triggerEnter += AgroRangeEnter;
        agroRangeScript.collisionEnter += AgroRangeEnter;
        agroRangeScript.triggerStay += AgroRangeStay;
        agroRangeScript.triggerExit += AgroRangeExit;
        attackRangeScript.triggerEnter += AttackRangeEnter;
        attackRangeScript.triggerStay += AttackRangeStay;
        attackRangeScript.triggerExit += AttackRangeExit;
        if(health!=null)
            health.dieEvent += Die;
    }

    /*
     * summary:
     * unsubscribing to the events
     * runs when the object is dies
     */
    public void DisableEventColliders(Action<Collider2D>[] subsriberEvents, Sensors[] sensors) {
        foreach (Sensors sensor in sensors) {
            foreach (Action<Collider2D> subsriberEvent in subsriberEvents) {
                sensor.triggerEnter -= subsriberEvent;
                sensor.triggerExit -= subsriberEvent;
            }
        }
        
    }
    /*
     * summary:
     * an event function that calls the function to unsubscribe to the events
     */
    public void Die() {
        DisableEventColliders(new Action<Collider2D>[] { AgroRangeEnter, AgroRangeStay, AgroRangeExit, AttackRangeEnter, AttackRangeExit }, new Sensors[] { agroRangeScript, attackRangeScript });
        health.dieEvent -= Die;
    }

}
