using UnityEngine;

public class AttackManager : State
{
    public AttackInfo currentAttack;
    public float attackTime;
    [SerializeField] protected float attackSpeedModifier = 1;
    protected virtual void Start() {
        clip = unitVariables.animations.parryAnimation;
    }
    public override void Enter() {
        clip = currentAttack.clip;
        base.Enter();
    }

    protected override void StateIsDone() {
        base.StateIsDone();
        //Debug.Log("Done: "+IsStateDone());
        animator.speed = 1;
    }
}
