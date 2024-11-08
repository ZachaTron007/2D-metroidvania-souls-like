using UnityEngine;

public class LedgeTurnIdelState : IdelState
{
    public override void UpdateState() {
        if (!unitVariables.IsGroundInFront()) {
            unitVariables.SetDirection(unitVariables.GetDirection() * -1);
            Debug.Log("Ran out of ground");
        }
    }
}
