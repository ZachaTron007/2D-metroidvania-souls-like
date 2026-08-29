using UnityEngine;

public abstract class RbVelocitySlerp : State {
    [SerializeField] protected float accSpeed;
    [SerializeField] protected float velPower;
    protected Vector2 dir;
    //protected float targetSpeed;
    protected Vector2 movement;
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = GetTargetSpeed();
        Vector2 speedDiffrence = new Vector2(targetSpeed - rb.linearVelocityX*dir.x, targetSpeed - rb.linearVelocityY * dir.y);
        movement = new Vector2(Mathf.Pow(Mathf.Abs(speedDiffrence.x) * accSpeed, velPower), Mathf.Pow(Mathf.Abs(speedDiffrence.y) * accSpeed, velPower)) * new Vector2(Mathf.Sign(speedDiffrence.x), Mathf.Sign(speedDiffrence.y));
        
    }
    protected abstract float GetTargetSpeed();
}
