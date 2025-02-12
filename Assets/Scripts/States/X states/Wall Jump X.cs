using UnityEngine;

public class WallJumpX : LerpingInhertedClass
{
    [SerializeField] private float speed;
    protected override void Start() {
        base.Start();
        clip = unitVariables.animations.jumpAnimation;
    }
    public override void Enter() {
        base.Enter();
        movment[0] = new RbVelocityLerp(startSpeed: speed, targetSpeed: 0, totalTime, dir: new Vector2(0,1), rb, curve, this);
    }
    public override void FinishedLerping() {
        StateIsDone();
    }
}
