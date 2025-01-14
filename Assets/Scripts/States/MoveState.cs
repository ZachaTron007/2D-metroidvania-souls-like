using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MoveState : State
{
    [SerializeField] private float moveSpeed = 300;
    public float weight = 1;
    [SerializeField] private float accSpeed;
    [SerializeField] private float velPower;

    public override void Enter() {
        base.Enter();
        animator.Play(unitVariables.animations.runAnimation.name);
    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = GetTargetSpeed();
        float speedDiffrence = targetSpeed - rb.linearVelocityX;
        float movement = Mathf.Pow(Mathf.Abs(speedDiffrence) * accSpeed, velPower) * Mathf.Sign(speedDiffrence);
        rb.AddForce(movement * Vector2.right);
    }
    protected virtual float GetTargetSpeed() {
        return moveSpeed * unitVariables.GetDirection()*weight;
    }
}
