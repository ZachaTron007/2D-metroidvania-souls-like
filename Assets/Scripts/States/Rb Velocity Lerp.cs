using UnityEngine;

public abstract class RbVelocityLerp : State
{
    [Header("Lerp Settings")]
    [SerializeField] protected float totalTime = 1;
    private Vector2 dir;
    private float startSpeed;
    //protected float targetSpeed;
    private Vector2 movement;
    [SerializeField] private float counter;
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = GetTargetSpeed();
        movement = HelperFunctions.LerpHelper(startValue: startSpeed, endValue: targetSpeed, totalTime: totalTime, counter:  ref counter) * dir;
        movement-= rb.linearVelocity*dir;
        rb.linearVelocity += movement;
        if (counter <= 0) { FinishedLerping(); }
        if(gameObject.name== "Wall Slide State")
        Debug.Log(movement);
    }
    protected abstract void FinishedLerping();
    protected void ResetLerp(float startSpeed, float totalTime, Vector2 dir) {
        this.startSpeed = startSpeed;
        this.totalTime = totalTime;
        counter = totalTime;
        this.dir = dir;
        
    }
    protected abstract float GetTargetSpeed();
}
