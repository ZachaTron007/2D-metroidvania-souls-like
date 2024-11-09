using UnityEngine;

public class BaseIdelState : State
{
    [SerializeField] protected AnimationClip idelAniamtion;

    public override void Enter() => animator.Play(idelAniamtion.name);
}
