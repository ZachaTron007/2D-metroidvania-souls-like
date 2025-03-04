using UnityEngine;

public class ChargeAttack : ParentMeleeAttack
{
    public override void Enter() {
        clip = unitVariables.animations.runAnimation;
        base.Enter();
        Attack();
    }
    private void Attack() {
        SetHitBoxStatus(true);
    }
    public override void Exit() {
        base.Exit();
    }
}
