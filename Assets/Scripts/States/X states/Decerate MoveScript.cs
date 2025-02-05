using UnityEngine;

public class DecelerateMoveScript : RbVelocityLerp
{
    public float exitSpeed = .3f;
    protected void Start() {
        clip = unitVariables.animations.runAnimation;
    }
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
