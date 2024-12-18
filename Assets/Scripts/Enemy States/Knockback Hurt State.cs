using UnityEngine;

public class KnockbackHurtState : HurtState
{
    [SerializeField] private float knockbackX = 0.5f;
    [SerializeField] private float knockbackY = 0.5f;
    [SerializeField] private float knockbackTime = 0.5f;
    [SerializeField] private float upTime = 0.5f;

    public override void Enter() {
        base.Enter();
        interuptable = .4f;
        rb.linearVelocity = new Vector2(-unitVariables.GetDirection() * knockbackX, knockbackY);
        Invoke(nameof(Stop), knockbackTime);
        rb.gravityScale = 0;
        Invoke(nameof(ReturnGravity), upTime);
    }

    private void Stop() {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 2;
    }
    private void ReturnGravity() => rb.linearVelocity = new Vector2(-unitVariables.GetDirection() * knockbackX, 0);
    

    public override void Exit() {
        base.Exit();
    }
    public override void FixedUpdateState() {
    }
}
