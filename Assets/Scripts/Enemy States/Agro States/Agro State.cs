using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroState : State
{

    [SerializeField] private float agroSpeed = 250;
    [SerializeField] protected AnimationClip runClip;

    public override void Enter() {
        
    }
    public override void UpdateState() {
        Run();
    }
    protected virtual void Run() {
        animator.Play(runClip.name);
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * agroSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
}
