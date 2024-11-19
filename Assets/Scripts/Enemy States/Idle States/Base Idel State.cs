using Unity.VisualScripting;
using UnityEngine;

public class BaseIdelState : State
{
    [SerializeField] protected AnimationClip idelAniamtion;
    [SerializeField] protected AnimationClip walkAnimation;
    [SerializeField] private float walkBackDistance;
    [SerializeField] protected float walkSpeed = 50;
    [SerializeField] private float forgetTime = 2;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private int startDir;
    [SerializeField] private bool needsRetreating=false;

    private void Start() {
        startPos = unitVariables.transform.position;
        startDir = unitVariables.GetDirection();
    }

    public override void Enter() => IdelFunctionality();

    public override void UpdateState() {
        if (Mathf.Abs(Mathf.Abs(unitVariables.transform.position.x) - startPos.x) > walkBackDistance) {
            needsRetreating = true;
        }
        if (needsRetreating) {
            Debug.Log(unitVariables.transform.position.x - startPos.x);
            unitVariables.SetDirection((unitVariables.transform.position.x - startPos.x > walkBackDistance) ? -1 : 1);
            rb.linearVelocity = new Vector2(unitVariables.GetDirection()*walkSpeed*Time.deltaTime, rb.linearVelocityY);
            animator.Play(walkAnimation.name);
        } else {
            IdelFunctionality();
        }
    }
    protected virtual void IdelFunctionality() {
        animator.Play(idelAniamtion.name);
    }
}
