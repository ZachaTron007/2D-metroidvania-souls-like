using UnityEngine;

public abstract class RbVelocityLerp : State
{
    [Header("Lerp Settings")]
    [SerializeField] protected float totalTime = 1;
    private Vector2 dir;
    private float startSpeed;
    private Vector2 movement;
    [SerializeField] private float counter;
    [SerializeField] private AnimationCurve curve;
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = GetTargetSpeed();
        movement = HelperFunctions.LerpHelper(startSpeed, targetSpeed, totalTime, ref counter, curve) * dir;
        movement-= rb.linearVelocity * new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        rb.linearVelocity += movement;
        if (counter <= 0) { FinishedLerping(); }
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
