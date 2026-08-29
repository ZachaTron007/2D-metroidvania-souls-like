using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroState : State
{

    [SerializeField] private float agroSpeed = 250;

    private void Start() {
        clip = unitVariables.animations.runAnimation;
    }
    public override void Enter() {
        
    }
    public override void UpdateState() {
        Run();
    }
    protected virtual void Run() {
        if (unitVariables.IsGroundInFront()) {
            animator.Play(unitVariables.animations.runAnimation.name);
            
            rb.linearVelocity = new Vector2(unitVariables.GetDirection() * agroSpeed * Time.deltaTime, rb.linearVelocity.y);
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.Play(unitVariables.animations.idelAnimation.name);
        }

    }
}
