using Unity.VisualScripting;
using UnityEngine;

public class BlockRecoverState : State
{
    [SerializeField] private GameObject dust;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private PhysicsMaterial2D frictionyMaterial;
    private PhysicsMaterial2D originalMaterial;
    [SerializeField] private float friction;
    [SerializeField] private Quaternion rotation;
    private GameObject slideDust;
    private float counter;
    protected void Start() {
        clip = unitVariables.animations.blockAnimation;
    }
    public override void Enter() {
        base.Enter();
        originalMaterial = rb.sharedMaterial;
        frictionyMaterial.friction = friction;
        rb.sharedMaterial = frictionyMaterial;
        interuptable = .3f;
        rb.linearVelocity = new Vector2(-unitVariables.GetDirection() * knockbackSpeed, rb.linearVelocity.y);
        slideDust = Instantiate(dust, this.transform);
        slideDust.transform.rotation = rotation;
        slideDust.GetComponent<SlideDustScript>().SetDirection(unitVariables.GetDirection());
        Invoke(nameof(StateIsDone), knockbackTime);

    }
    protected override void StateIsDone() {
        base.StateIsDone();
        rb.sharedMaterial = originalMaterial;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        Destroy(slideDust);
    }


}
