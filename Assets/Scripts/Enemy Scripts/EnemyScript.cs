using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyScript : Unit {
    protected float maxDist = .3f;
    [Header("Awareness Colliders")]
    //[SerializeField] protected GameObject[] AwarenessColliders = new GameObject[3];
    [SerializeField] protected GameObject awarenessColliderParent;
    [SerializeField] protected List<GameObject> awarenessColliders = new List<GameObject>();
    protected BoxCollider2D[] hitboxes = new BoxCollider2D[3];
    protected Sensors[] sensors = new Sensors[3];
    protected bool isWithinAgroRange = false;
    protected bool isWithinAttackRange = false;
    [SerializeField] private float agroDelay = .5f;
    private float agroDelayCounter;

    private delegate void EventDelegate(Collider2D other);
    private EventDelegate[,] listOfEvents;
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
        listOfEvents = new EventDelegate[,] { { AgroRangeEnter, null, null }, { null, AgroRangeStay, AgroRangeExit }, { AttackRangeEnter, AttackRangeStay, AttackRangeExit } };

        List<Sensors> colliders = new List<Sensors>();
        awarenessColliderParent.GetComponentsInChildren(true, colliders);
        for (int i = 0; i < colliders.Count; i++) {
            int j = 1;//collider.gameObject.layer-10;
            //sensors[j] = collider.GetComponent<Sensors>();
            //sensors[j] += listOfEvents[]
            hitboxes[j] = awarenessColliders[i].GetComponent<BoxCollider2D>();
            //if(collider.gameObject.layer)
        }

        for (int i = 0; i < 3; i++) {
            sensors[i] = awarenessColliders[i].GetComponent<Sensors>();
            hitboxes[i] = awarenessColliders[i].GetComponent<BoxCollider2D>();
        }

        sensors[(int)ColliderType.agro].triggerEnter += AgroRangeEnter;
        sensors[(int)ColliderType.agro].triggerStay += AgroRangeStayEnter;
        sensors[(int)ColliderType.deAgro].triggerStay += AgroRangeStay;
        sensors[(int)ColliderType.deAgro].triggerExit += AgroRangeExit;
        sensors[(int)ColliderType.attack].triggerEnter += AttackRangeEnter;
        sensors[(int)ColliderType.attack].triggerStay += AttackRangeStay;
        sensors[(int)ColliderType.attack].triggerExit += AttackRangeExit;
        if (health != null)
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
        DisableEventColliders(new Action<Collider2D>[] { AgroRangeEnter, AgroRangeStayEnter, AgroRangeStay, AgroRangeExit, AttackRangeEnter, AttackRangeExit }, sensors);
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

    protected void AgroRangeStayEnter(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            agroDelayCounter += Time.deltaTime;
            if (agroDelayCounter >= agroDelay) {
                isWithinAgroRange = true;
                Vector2 directionOfPlayer = other.gameObject.transform.position - gameObject.transform.position;
                SetDirection(ShouldSwitchDirection(GetDirection(), directionOfPlayer));
            }
        }
    }
    protected void AgroRangeStay(Collider2D other) {
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
    protected void AgroRangeExit(Collider2D other) {
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
