using Unity.VisualScripting;
using UnityEngine;

public class BaseIdelState : State
{
    [SerializeField] protected AnimationClip idelAniamtion;
    [SerializeField] protected AnimationClip walkAnimation;
    [SerializeField] private float walkBackDistance = 10;
    [SerializeField] protected float walkSpeed = 500;
    [SerializeField] private float forgetTime = 4;
    private Vector2 startPos;
    private int startDir;
    private bool needsRetreating = false;
    private float counter = 0;
    protected delegate void IdelFunctionalityDelegate();
    protected IdelFunctionalityDelegate IdelFunctionality;

    protected override void Start() {
        Debug.Log(unitVariables);
        startPos = unitVariables.transform.position;
        startDir = unitVariables.GetDirection();
    }

    public override void Enter() {
        IdelFunctionality = Stay;
        counter = 0;
    }
    protected void Walk() {
        animator.Play(walkAnimation.name);
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * walkSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
    protected void Retreating() {
        unitVariables.SetDirection((unitVariables.transform.position.x - startPos.x > walkBackDistance) ? -1 : 1);
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * walkSpeed * Time.deltaTime, rb.linearVelocityY);
        animator.Play(walkAnimation.name);
    }
    public override void UpdateState() {
        IdelFunctionality();
        float distance = Mathf.Abs(HelperFunctions.PointToDistance(unitVariables.transform.position, startPos));
        //Debug.Log(distance);
        if (distance> walkBackDistance) {
            counter+=Time.deltaTime;
            if (counter > forgetTime) {
                needsRetreating = true;
                counter = 0;
            }
        } else if (distance <= 1f) {
            needsRetreating = false;
        }
        if (needsRetreating) {
            if (IdelFunctionality != Retreating)    IdelFunctionality = Retreating;

        } else {
            if (IdelFunctionality == Retreating) {
                IdelFunctionality = Stay;
                //unitVariables.SetDirection(startDir);
            }
            
        }
    }
    protected virtual void Stay() {
        rb.linearVelocity = Vector2.zero;
        animator.Play(idelAniamtion.name);
        
    }
}
