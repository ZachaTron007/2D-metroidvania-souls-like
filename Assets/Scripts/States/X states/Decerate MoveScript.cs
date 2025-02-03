using UnityEngine;

public class DecelerateMoveScript : RbVelocityLerp
{
    [SerializeField] private float decerationSpeed = 1;
    public float exitSpeed = 1;

    public override void Enter() {
        base.Enter();
        ResetLerp(rb.linearVelocity.x, totalTime, Vector2.right);
    }
    protected override float GetTargetSpeed() {
        return 0;
    }
    protected override void FinishedLerping() {
    }

}
