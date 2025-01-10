using UnityEngine;

public class ParryRecoverState : State
{
    [SerializeField] private float parryRecoverTime;
    private float parryRecoverTimer;

    public override void Enter() {
        base.Enter();
        interuptable = 1f;
        animator.Play(unitVariables.animations.HurtAnimation.name);

    }

    public override void UpdateState() {
        base.UpdateState();
        parryRecoverTimer += Time.deltaTime;
        if (parryRecoverTimer >= parryRecoverTime) {
            Exit();
            parryRecoverTimer = 0;
        }
    }
}
