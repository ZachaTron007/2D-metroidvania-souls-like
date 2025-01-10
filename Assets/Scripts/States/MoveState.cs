using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MoveState : State
{
    [SerializeField] private float moveSpeed = 300;
    [SerializeField] private float accSpeed;
    [SerializeField] private float velPower;

    public override void Enter() {
        base.Enter();
        animator.Play(unitVariables.animations.runAnimation.name);
    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float targetSpeed = moveSpeed * unitVariables.GetDirection();
        float speedDiffrence = targetSpeed - rb.linearVelocityX;
        float movement = Mathf.Pow(Mathf.Abs(speedDiffrence) * accSpeed, velPower) * Mathf.Sign(speedDiffrence);//playerVariables.GetDirection();
        //movement = Mathf.Abs(speedDiffrence) * accSpeed * playerVariables.GetDirection();
        rb.AddForce(movement * Vector2.right);

        //rb.linearVelocity = new Vector2(playerVariables.moveVetcor.x * moveSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }
}
