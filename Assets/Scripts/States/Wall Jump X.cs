using UnityEngine;

public class WallJumpX : MoveState
{
    [SerializeField] private float totalSpeedTransfer = .5f;
    [SerializeField] private float forceAdded = 2;
    private float counter;
    public override void Enter() {
        base.Enter();
        interuptable = .1f;
        counter = totalSpeedTransfer;
        rb.AddForce(new Vector2(forceAdded,0));

    }

    public override void UpdateState() {
        //Debug.Log(weight);
        counter -= Time.deltaTime;
        weight = Mathf.Lerp(0,1 ,1-(counter/totalSpeedTransfer));
        if (weight == 1) Exit();
            
    }
}
