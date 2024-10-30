using UnityEngine;

public class ParryState : State
{
    [SerializeField] private AnimationClip parryClip;
    public override void Enter() {
        animator.Play(parryClip.name);
        
    }
}
