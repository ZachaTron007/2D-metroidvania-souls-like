using UnityEngine;

public class DeathScript : State
{
    [SerializeField] private AnimationClip deathClip;
    [SerializeField] private float startTime = 4;
    private float times = 0;
    [SerializeField] private float speed = 0.2f;
    public override void Enter() {
        interuptable = false;
        animator.Play(deathClip.name);
        times = 0;
        Debug.Log("Death");

    }

    public override void FixedUpdateState() {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void UpdateState() {
        
        if (times >= startTime) {
            times += Time.deltaTime*speed;
            float alpha = 1 - (times-startTime);
            unitVariables.sr.color = new Color(unitVariables.sr.color.r, unitVariables.sr.color.g, unitVariables.sr.color.b, alpha);
            if (unitVariables.sr.color.a <= 0) {
                Destroy(unitVariables.gameObject);
            }
        } else {
            times += Time.deltaTime;
        }
    }
}
