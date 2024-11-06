using UnityEngine;

public class BlockRecoverState : State
{
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] private GameObject dust;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    SlideDustScript slidedust;
    private float counter;

    public override void Enter() {
        interuptable = false;
        rb.AddForce(Vector2.right * knockbackSpeed * -unitVariables.GetDirection() * Time.deltaTime);
        //slideDust = Instantiate<SlideDustScript>(dust,this.transform, new Vector3(0, 0, 90));
        Invoke("Exit", knockbackTime);

    }
    public override void FixedUpdateState() {
        //rb.linearVelocity = ;
    }

    public override void Exit() {
        //slidedust.Destroy();
    }


}
