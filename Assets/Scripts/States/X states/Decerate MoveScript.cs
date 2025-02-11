using UnityEngine;

public class DecelerateMoveScript : LerpingInhertedClass
{
    public float exitSpeed = .3f;
    protected override void Start() {
        base.Start();
        clip = unitVariables.animations.runAnimation;
    }
    public override void Enter() {
        base.Enter();
        movment[0] = new RbVelocityLerp(startSpeed: rb.linearVelocity.x, targetSpeed: 0, totalTime, dir: Vector2.right,rb, curve);
    }


}
