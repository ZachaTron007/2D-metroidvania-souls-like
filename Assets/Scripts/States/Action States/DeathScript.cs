using UnityEngine;

public class DeathScript : State
{
    [SerializeField] private float startTime = 4;
    private float times = 0;
    [SerializeField] private float speed = 0.2f;
    protected void Start() {
        clip = unitVariables.animations.deathAnimation;
    }

    public override void Enter() {
        base.Enter();
        interuptable = 1;
        times = 0;
        Debug.Log("Death");

    }

    public override void FixedUpdateState() {
        base.FixedUpdateState();
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void UpdateState() {
        base.UpdateState();
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
