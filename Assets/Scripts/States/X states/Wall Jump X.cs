using UnityEngine;

public class WallJumpX : LerpingInhertedClass
{
    [SerializeField] private float speed;
    private RbVelocityLerp horizontalMovment;
    private void Start() {
        clip = unitVariables.animations.jumpAnimation;
    }
    public override void Enter() {
        base.Enter();
        interuptable = .1f;
        horizontalMovment = new RbVelocityLerp(startSpeed: speed, targetSpeed: 0,totalTime, new Vector2(0,1),rb, curve, this);
    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        horizontalMovment.FixedUpdate();
    }
    public override void FinishedLerping() {
        StateIsDone();
    }
}
