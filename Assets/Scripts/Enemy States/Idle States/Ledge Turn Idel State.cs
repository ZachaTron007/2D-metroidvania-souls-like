using UnityEngine;

public class LedgeTurnIdelState : IdelState
{
    public override void UpdateState() {
        base.UpdateState();
        if(!unitVariables.IsGroundInFront()) {
            unitVariables.SetDirection(-unitVariables.GetDirection());
        }
    }
}
