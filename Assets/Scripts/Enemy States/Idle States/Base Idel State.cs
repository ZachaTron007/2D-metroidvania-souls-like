using Unity.VisualScripting;
using UnityEngine;

public class BaseIdelState : State
{
    [SerializeField] protected AnimationClip idelAniamtion;
    [SerializeField] protected AnimationClip walkAnimation;
    [SerializeField] private float walkBackDistance;
    [SerializeField] protected float walkSpeed = 500;
    [SerializeField] private float forgetTime = 4;
    private Vector2 startPos;
    private int startDir;
    private bool needsRetreating = false;
    private float counter = 0;
    protected delegate void IdelFunctionalityDelegate();
    protected IdelFunctionalityDelegate IdelFunctionality;

    private void Start() {
        
        startPos = unitVariables.transform.position;
        Debug.Log(unitVariables);
        startDir = unitVariables.GetDirection();
    }

    public override void Enter() {
        IdelFunctionality = Stay;
        counter = 0;
    }
    public override void UpdateState() {
        float distance = Mathf.Abs(HelperFunctions.PointToDistance(unitVariables.transform.position, startPos));
        if (distance> walkBackDistance) {
            counter+=Time.deltaTime;
            if (counter > forgetTime) {
                needsRetreating = true;
                counter = 0;
            }
        } else if (distance <= .5f) {
            needsRetreating = false;
        }
        if (needsRetreating) {
            unitVariables.SetDirection((unitVariables.transform.position.x - startPos.x > walkBackDistance) ? -1 : 1);
            rb.linearVelocity = new Vector2(unitVariables.GetDirection()*walkSpeed*Time.deltaTime, rb.linearVelocityY);
            animator.Play(walkAnimation.name);
        } else {
            IdelFunctionality();
            unitVariables.SetDirection(startDir);
        }
    }
    protected virtual void Stay() {
        rb.linearVelocity = Vector2.zero;
        animator.Play(idelAniamtion.name);
        
    }
}
