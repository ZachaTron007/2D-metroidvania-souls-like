using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyScript : Unit {
    protected float maxDist = .3f;
    [Header("Awareness Colliders")]
    [SerializeField] protected GameObject awarenessColliderParent;
    protected List<BoxCollider2D> hitboxes = new List<BoxCollider2D> { };
    protected List<Sensors> sensors = new List<Sensors> { };
    protected bool isWithinAgroRange = false;
    protected bool isWithinAttackRange = false;
    [SerializeField] private float agroDelay = .5f;
    private float agroDelayCounter;

    private Action<Collider2D>[,] listOfEvents;
    private enum ColliderType {
        agro,
        deAgro,
        attack
    }
    /*
     * summary:
     * subscribing to the events
     * runs when the object is created
     */
    protected void AgroAttackColliders() {
        //sets the list of colldier functions
        listOfEvents = new Action<Collider2D>[,] { { AgroRangeEnter, AgroRangeStay, null }, { null , DeAgroRangeStay, DeAgroRangeExit }, { AttackRangeEnter, AttackRangeStay, AttackRangeExit } };
        //gets a list of the awarness colliders
        List<Sensors> colliders = new List<Sensors>();
        awarenessColliderParent.GetComponentsInChildren(true, colliders);
        int j = 0;
        for (int i = 0; i < colliders.Count; i++) {
            //normalizes the pointer to the awarness collider
            int EventLoctor = colliders[i].gameObject.layer - HelperFunctions.layers["Agro hitBox"];
            //if in range of the amount of colliders
            if (EventLoctor >= 0 && EventLoctor < listOfEvents.Length) {
                /*
                 * Assiging Events and Colliders
                 */
                sensors.Add(colliders[i].GetComponent<Sensors>());
                sensors[j].triggerEnter += listOfEvents[EventLoctor, 0];
                sensors[j].triggerStay += listOfEvents[EventLoctor, 1];
                sensors[j].triggerExit += listOfEvents[EventLoctor, 2];
                hitboxes.Add(colliders[i].GetComponent<BoxCollider2D>());
            } else break;
            j++;
        }
        if (health != null)
            health.dieEvent += Die;
    }

    /*
     * summary:
     * unsubscribing to the events
     * runs when the object is dies
     */
    public void DisableEventColliders(Action<Collider2D>[] subsriberEvents, List<Sensors> sensors) {
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
        DisableEventColliders(new Action<Collider2D>[] { AgroRangeEnter, AgroRangeStay, DeAgroRangeStay, DeAgroRangeExit, AttackRangeEnter, AttackRangeExit }, sensors);
        health.dieEvent -= Die;
    }
    /*
     * summary:
     * collider enter and exit events for agro and attack range
     */
    protected static void AgroRangeEnter(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            //isWithinAgroRange = true;

        }
    }

    protected void AgroRangeStay(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            agroDelayCounter += Time.deltaTime;
            if (agroDelayCounter >= agroDelay) {
                isWithinAgroRange = true;
                Vector2 directionOfPlayer = other.gameObject.transform.position - gameObject.transform.position;
                SetDirection(ShouldSwitchDirection(GetDirection(), directionOfPlayer));
            }
        }
    }
    protected void DeAgroRangeStay(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            //isWithinAgroRange = true;
            if (isWithinAgroRange) {
                Vector2 directionOfPlayer = other.gameObject.transform.position - gameObject.transform.position;
                int dir = ShouldSwitchDirection(GetDirection(), directionOfPlayer);
                SetDirection(dir);
                isWithinAgroRange = IsPlayerBlocked(directionOfPlayer);
            }
        }
    }
    protected void DeAgroRangeExit(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            agroDelayCounter = 0;
            isWithinAgroRange = false;
        }
    }

    protected void AttackRangeEnter(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
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
        if (Mathf.Abs(PlayerDirection.x) > turnGracePeriod && state.interuptable == 0) return PlayerDirection.x > 0 ? 1 : -1;
        return direction;
    }
    /*
     * summary:
     * if agroed used to check to see if you should actually be agroed
     */
    private bool IsPlayerBlocked(Vector2 PlayerDirection) {
        float distance = Mathf.Sqrt((PlayerDirection.x * PlayerDirection.x) + (PlayerDirection.y * PlayerDirection.y));
        
        RaycastHit2D hit = ShootRayDirection(PlayerDirection, HelperFunctions.layers["Player"], distance,debugRay:true);
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

    
    

}
