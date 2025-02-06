using UnityEngine;

public class WallJumpX : RbVelocityLerp
{
    [SerializeField] private float speed;
    private void Start() {
        clip = unitVariables.animations.jumpAnimation;
    }
    public override void Enter() {
        base.Enter();
        interuptable = .1f;
        ResetLerp(speed, totalTime,Vector2.right);
    }
    protected override float GetTargetSpeed() {
        return 0;
    }
    protected override void FinishedLerping() {
        StateIsDone();
    }
}
