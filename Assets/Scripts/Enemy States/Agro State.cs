using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroState : State
{

    [SerializeField] private float agroSpeed = 250;
    [SerializeField] private AnimationClip runClip;
    public override void Enter() {
        animator.Play(runClip.name);
    }
    public override void FixedUpdateState() {

        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * agroSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }

    public override void UpdateState() {
        
    }
}
