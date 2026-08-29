using UnityEngine;

public class AgroStateRetreat : AgroStateStay
{
    private float counter;
    [SerializeField] private float timeToRetreat = 5;

    public override void Exit() {
        base.Exit();
        counter = 0;
    }

    protected override void DistanceOver() {
        base.DistanceOver();
        counter += Time.deltaTime;
        if (counter > timeToRetreat) {
            counter = 0;
            Exit();
        }
    }
}
