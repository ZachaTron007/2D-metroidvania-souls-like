using UnityEngine;

public class ParryRecoverState : State
{
    [SerializeField] private AnimationClip parryRecoverAnimation;
    [SerializeField] private float parryRecoverTime;
    private float parryRecoverTimer;

    public override void Enter() {
        interuptable = 1f;
        animator.Play(parryRecoverAnimation.name);

    }

    public override void Exit() { 
        stateDone = true;
    }

    public override void UpdateState() {
        parryRecoverTimer += Time.deltaTime;
        if (parryRecoverTimer >= parryRecoverTime) {
            Exit();
            parryRecoverTimer = 0;
        }
    }
}
