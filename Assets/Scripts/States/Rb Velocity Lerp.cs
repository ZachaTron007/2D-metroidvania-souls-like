using UnityEngine;

public abstract class RbVelocityLerp : State
{
    [Header("Lerp Settings")]
    [SerializeField] protected float totalTime = 1;
    protected Vector2 dir;
    protected float startSpeed;
    //protected float targetSpeed;
    protected Vector2 movement;
    [SerializeField] protected float counter;
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = GetTargetSpeed();
        movement = HelperFunctions.LerpHelper(startValue: startSpeed, endValue: targetSpeed, totalTime: totalTime, counter:  ref counter) * dir;
        movement-= rb.linearVelocity;
        rb.linearVelocity += movement;
        if (counter <= 0) { FinishedLerping(); }
        //Debug.Log(targetSpeed);


    }
    protected abstract void FinishedLerping();
    protected void ResetLerp(float startSpeed, float totalTime) {
        this.startSpeed = startSpeed;
        this.totalTime = totalTime;
        counter = totalTime;
        
    }
    protected abstract float GetTargetSpeed();
}
