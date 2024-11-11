using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EnemyScript : Unit {
    protected float maxDist = .3f;
    [Header("Awareness Colliders")]
    [SerializeField] protected GameObject[] AwarenessColliders = new GameObject[3];
    protected BoxCollider2D[] hitboxes = new BoxCollider2D[3];
    protected Sensors[] sensors = new Sensors[3];
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


    private enum ColliderType {
        agro,
        deAgro,
        attack
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
        if (other.gameObject.tag == "Player"&&isWithinAgroRange == true) {
            Vector2 directionOfPlayer = other.gameObject.transform.position - gameObject.transform.position;
            direction = ShouldSwitchDirection(direction,directionOfPlayer);
            isWithinAgroRange = !IsPlayerBlocked(directionOfPlayer);
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
        float turnGracePeriod = mainCollider.hitBox.size.x / 2;
        if (Mathf.Abs(PlayerDirection.x) > turnGracePeriod && state.interuptable) {
            direction = PlayerDirection.x > 0 ? 1 : -1;
            SetDirection(direction);
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
    public override void SetDirection(int newDirection) {
        base.SetDirection(newDirection);
        foreach (BoxCollider2D collider in hitboxes) {
            collider.offset = new Vector2(Mathf.Abs(collider.offset.x) * newDirection, collider.offset.y);
        }
    }

    
    
    
    /*
     * summary:
     * subscribing to the events
     * runs when the object is created
     */
    protected void AgroAttackColliders() {
        for (int i = 0; i < 3; i++) {
            sensors[i] = AwarenessColliders[i].GetComponent<Sensors>();
            hitboxes[i] = AwarenessColliders[i].GetComponent<BoxCollider2D>();
  
        }
        sensors[(int)ColliderType.agro].triggerEnter += AgroRangeEnter;
        sensors[(int)ColliderType.agro].collisionEnter += AgroRangeEnter;
        sensors[(int)ColliderType.deAgro].triggerStay += AgroRangeStay;
        sensors[(int)ColliderType.deAgro].triggerExit += AgroRangeExit;
        sensors[(int)ColliderType.attack].triggerEnter += AttackRangeEnter;
        sensors[(int)ColliderType.attack].triggerStay += AttackRangeStay;
        sensors[(int)ColliderType.attack].triggerExit += AttackRangeExit;
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
    protected override void EventUnsubscribe() {
        base.EventUnsubscribe();
        DisableEventColliders(new Action<Collider2D>[] { AgroRangeEnter, AgroRangeStay, AgroRangeExit, AttackRangeEnter, AttackRangeExit }, sensors);
        health.dieEvent -= Die;
    }
    

}
