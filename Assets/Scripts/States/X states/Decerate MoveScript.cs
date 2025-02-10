using UnityEngine;

public class DecelerateMoveScript : LerpingInhertedClass
{
    public float exitSpeed = .3f;
    private RbVelocityLerp horizontalMovment;
    protected void Start() {
        clip = unitVariables.animations.runAnimation;
    }
    public override void Enter() {
        base.Enter();
        horizontalMovment = new RbVelocityLerp(startSpeed: rb.linearVelocity.x, targetSpeed: 0, totalTime, dir: Vector2.right,rb, curve);
    }

}
