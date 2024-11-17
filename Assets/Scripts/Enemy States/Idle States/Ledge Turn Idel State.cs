using UnityEngine;

public class LedgeTurnIdelState : IdelState
{
    public override void UpdateState() {
        //Debug.Log("Is ground in front: " + unitVariables.IsGroundInFront());
        if(!unitVariables.IsGroundInFront()) {
            unitVariables.SetDirection(-unitVariables.GetDirection());
        }
    }
}
