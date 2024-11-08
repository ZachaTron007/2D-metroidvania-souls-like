using UnityEngine;

public class BaseIdelState : State
{
    [SerializeField] protected AnimationClip idelAniamtion;

    public override void Enter() {
        rb.linearVelocity = Vector2.zero;
        animator.Play(idelAniamtion.name);
    }
}
