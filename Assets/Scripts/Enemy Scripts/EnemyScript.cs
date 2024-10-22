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

    protected bool WallCheck() {
        //layers to hit
        int layerNumber = 6;
        RaycastHit2D hit = ShootRay(direction, layerNumber, maxDist);
        if (hit) {
            return true;
        }
        
        return false;
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
            direction = ShouldSwitchDirection(direction,directionOfPlayer);
            isWithinAgroRange = IsPlayerBlocked(directionOfPlayer);
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
     * checks to see if the enemy should turn around
     * used if the enemy is agroed
     */
    private int ShouldSwitchDirection(int direction,Vector2 PlayerDirection) {
        float turnGracePeriod = mainCollider.size.x / 2;
        if (Mathf.Abs(PlayerDirection.x) > turnGracePeriod && state.interuptable) {
            int tempDirection = direction;
            direction = PlayerDirection.x > 0 ? 1 : -1;
            if (tempDirection != direction) {
                agroRangeHitbox.offset *= new Vector2(-1, 1);
                attackRangeHitbox.offset *= new Vector2(-1, 1);
            }
            return direction;
        }

        return direction;
    }
    /*
     * summary:
     * if agroed used to check to see if you should actually be agroed
     */
    private bool IsPlayerBlocked(Vector2 PlayerDirection) {
        float distance = Mathf.Sqrt((PlayerDirection.x * PlayerDirection.x) + (PlayerDirection.y * PlayerDirection.y));
        RaycastHit2D hit = ShootRayDirection(PlayerDirection, 6, distance);
        if (hit) {
            if (hit.collider.tag == "Player") {
                return true;
            }
        }
        return false;
    }
    
    protected int switchDirection() {
        direction *= -1;
        agroRangeHitbox.offset *= new Vector2(-1, 1);
        attackRangeHitbox.offset *= new Vector2(-1, 1);

        return direction;
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
        EventUnsubscribe();
        
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
