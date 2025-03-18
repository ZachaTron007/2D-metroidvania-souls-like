using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class ParentMeleeAttack : State {

    public AttackInfo currentAttack;
    public float attackTime;
    [SerializeField] protected float attackSpeedModifier = 1;
    //[SerializeField] private float attackSpeed = 1;
    public Vector2 lookDirection;

    public virtual void HitCollided(Unit hitUnit) {

    }

    protected void SetHitBoxStatus(bool status) {
        currentAttack.attackHitBox.enabled = status;
    }
    protected virtual void Start() {
        currentAttack = GetComponent<AttackInfo>();
        clip = currentAttack.clip;
        interuptable = .8f;
    }
    public override void Enter() {
        base.Enter();
        unitVariables.currentAttack = this;
        animator.speed = currentAttack.speed;
    }
    protected Vector2 offsetVector() {
        return new Vector2(unitVariables.GetDirection() * Mathf.Abs(currentAttack.attackHitBox.offset.x), Mathf.Abs(currentAttack.attackHitBox.offset.y));
    }
    
    protected override void StateIsDone() {
        base.StateIsDone();
        SetHitBoxStatus(false);
        animator.speed = 1;
    }

}
