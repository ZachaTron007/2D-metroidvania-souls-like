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
        interuptable = .1f;
        //rb.linearVelocity += new Vector2(speed, 0);
        movment[0] = new RbVelocityLerp(startSpeed: speed, targetSpeed: 0, totalTime, dir: Vector2.right, rb, curve, this);
        movment[0].ResetLerp(startSpeed: speed, targetSpeed: 0, totalTime, dir: Vector2.right);
       // Debug.Log("Target: "+movment[0].targetSpeed+", Start" + movment[0].startSpeed);
        
    }
    public override void FinishedLerping() {
        //Debug.Log("should be exiting");
        //movment[0] = null;
        StateIsDone();
    }
}
