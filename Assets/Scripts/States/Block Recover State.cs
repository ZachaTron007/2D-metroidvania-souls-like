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

    public override void Enter() {
        originalMaterial = rb.sharedMaterial;
        frictionyMaterial.friction = friction;
        rb.sharedMaterial = frictionyMaterial;
        interuptable = false;
        rb.linearVelocity = new Vector2(-unitVariables.GetDirection() * knockbackSpeed, rb.linearVelocity.y);
        slideDust = Instantiate(dust, this.transform);//, rotation);
        slideDust.transform.rotation = rotation;
        Invoke(nameof(Exit), knockbackTime);

    }
    public override void Exit() {
        rb.sharedMaterial = originalMaterial;
        stateDone = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        Destroy(slideDust);
    }


}
