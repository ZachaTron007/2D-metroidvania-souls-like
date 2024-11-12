using UnityEngine;

public class DeathScript : State
{
    [SerializeField] private AnimationClip deathClip;
    [SerializeField] private float startTime;
    [SerializeField] private float time;
    [SerializeField] private float startAlpha;
    public override void Enter() {
        interuptable = false;
        animator.Play(deathClip.name);
        startTime = 1f;
        time = startTime;
        startAlpha = unitVariables.sr.color.a;

    }

    public override void FixedUpdateState() {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void UpdateState() {
        DEbu
        Debug.Log(Mathf.Lerp(startAlpha, 0, 1 - (time / startTime)));

        unitVariables.sr.color = new Color(unitVariables.sr.color.r, unitVariables.sr.color.g, unitVariables.sr.color.b, Mathf.Lerp(startAlpha, 0, 1 - (time / startTime)));
    }
}
