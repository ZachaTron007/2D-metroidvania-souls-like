using UnityEngine;

public class DeathScript : State
{
    [SerializeField] private AnimationClip deathClip;
    public override void Enter() {
        interuptable = false;
        animator.Play(deathClip.name);

    }

    public override void FixedUpdateState() {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
