using UnityEngine;

public abstract class RbVelocityLerp : State {
    [SerializeField] protected float accSpeed;
    [SerializeField] protected float velPower;
    //protected float targetSpeed;
    protected float movement;
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = GetTargetSpeed();
        float speedDiffrence = targetSpeed - rb.linearVelocityX;
        movement = Mathf.Pow(Mathf.Abs(speedDiffrence) * accSpeed, velPower) * Mathf.Sign(speedDiffrence);
        
    }
    protected abstract float GetTargetSpeed();
}
