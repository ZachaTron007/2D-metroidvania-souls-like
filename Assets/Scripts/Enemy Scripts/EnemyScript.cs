using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EnemyScript : Unit {
    protected float maxDist = .3f;
    [SerializeField] protected Sensors agroRange;
    [SerializeField] protected Sensors attackRange;
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
    protected void AgroRangeStay(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Vector2 directionOfPlayer = other.gameObject.transform.position - gameObject.transform.position;
            direction = directionOfPlayer.x > direction ? 1 : -1;
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
        
        agroRange.triggerEnter += AgroRangeEnter;
        agroRange.triggerStay += AgroRangeStay;
        agroRange.triggerExit += AgroRangeExit;
        attackRange.triggerEnter += AttackRangeEnter;
        attackRange.triggerStay += AttackRangeStay;
        attackRange.triggerExit += AttackRangeExit;
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
        DisableEventColliders(new Action<Collider2D>[] { AgroRangeEnter, AgroRangeStay, AgroRangeExit, AttackRangeEnter, AttackRangeExit }, new Sensors[] { agroRange, attackRange });
        health.dieEvent -= Die;
    }

}
