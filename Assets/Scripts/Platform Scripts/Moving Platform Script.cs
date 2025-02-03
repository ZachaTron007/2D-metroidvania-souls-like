using System;
using UnityEngine;

public class MovingPlatformScript : CustomPlatformBase
{
    [SerializeField] protected float moveSpeed = 10;
    [SerializeField] private Vector2 endPoint;
    private Vector2 endPoint2;
    private Rigidbody2D rb;
    [SerializeField] private float StayTime = 2;
    private float counter;
    protected float totalTime;
    private float moveCounter;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        endPoint2 = transform.position;
        totalTime = HelperFunctions.PointToDistance(endPoint2, endPoint)*moveSpeed;
        counter = totalTime;
    }

    void FixedUpdate()
    {
        float deltaX = HelperFunctions.LerpHelper(endPoint.x, endPoint2.x, totalTime, ref counter);// - transform.position.x;
        float deltaY = HelperFunctions.LerpHelper(endPoint.y, endPoint2.y, totalTime, ref counter);// - transform.position.y;
        ForceAppliedToUnits(new Vector2(transform.position.x - deltaX,transform.position.y - deltaY) / Time.fixedDeltaTime);
        transform.position = new Vector2(deltaX, deltaY);
        //rb.linearVelocity = new Vector2(deltaX, deltaY);
        
        if (counter <= 0) {
            moveCounter += Time.deltaTime;
            if(moveCounter > StayTime) ResetCounter();
        }

    }
    protected virtual void ResetCounter() {
        moveCounter = 0;
        counter = totalTime;
        Vector2 tempEndPoint = endPoint;
        endPoint = endPoint2;
        endPoint2 = tempEndPoint;
    }

    public override void StayOnCustomPlatform(Rigidbody2D playerRB) {
        base.StayOnCustomPlatform(playerRB);
        ForceAppliedToUnits(direction, GetSpeed());
    }
    public override void EnterOnCustomPlatform(Rigidbody2D playerRB) {
        base.EnterOnCustomPlatform(playerRB);
    }
    public override void ExitOnCustomPlatform(Rigidbody2D playerRB) {
        base.EnterOnCustomPlatform(playerRB);
    }

    
}
