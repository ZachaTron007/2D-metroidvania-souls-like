using UnityEngine;

public class FlyingAgroState : AgroState
{
    public override void FixedUpdateState() {
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * agroSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }
}
