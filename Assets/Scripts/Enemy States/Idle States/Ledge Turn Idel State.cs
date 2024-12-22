using UnityEngine;

public class LedgeTurnIdelState : IdelState
{
    public override void UpdateState() {
        base.UpdateState();
        if(!unitVariables.IsGroundInFront()&&unitVariables.GetGroundedState()) {
            unitVariables.SetDirection(-unitVariables.GetDirection());
        }
    }
}
