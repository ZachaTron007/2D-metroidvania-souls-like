using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParentMeleeAttack : State {

    public AttackInfo currentAttack;
    public float attackTime;
    [SerializeField] protected float attackSpeedModifier = 1;
    //[SerializeField] private float attackSpeed = 1;
    public Vector2 lookDirection;
    
    protected void SetHitBoxStatus(bool status) {
        unitVariables.mainCollider.enabled = status;
    }
    protected virtual void Start() {
        clip = unitVariables.animations.parryAnimation;
        interuptable = .8f;
    }
    public override void Enter() {
        clip = currentAttack.clip;
        base.Enter();
        animator.speed = currentAttack.speed;
    }
    protected Vector2 offsetVector() {
        return new Vector2(unitVariables.GetDirection() * Mathf.Abs(currentAttack.attackHitBox.offset.x), Mathf.Abs(currentAttack.attackHitBox.offset.y));
    }
    
    protected override void StateIsDone() {
        base.StateIsDone();
        //Debug.Log("Done: "+IsStateDone());
        SetHitBoxStatus(false);
        animator.speed = 1;
    }

}
