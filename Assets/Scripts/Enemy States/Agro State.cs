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

        rb.velocity = new Vector2(unitVariables.direction * agroSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    public override void UpdateState() {
        
    }
}
