using Unity.VisualScripting;
using UnityEngine;

public class BaseIdelState : State
{
    [SerializeField] private float walkBackDistance = 10;
    [SerializeField] protected float walkSpeed = 200;
    [SerializeField] private float forgetTime = 4;
    public Vector2 startPos;
    private int startDir;
    private bool needsRetreating = false;
    private bool reset;
    private float counter = 0;
    protected delegate void IdelFunctionalityDelegate();
    protected IdelFunctionalityDelegate IdelFunctionality;

    private void Start() {
        startPos = unitVariables.transform.localPosition;
        startDir = unitVariables.GetDirection();
        clip = unitVariables.animations.idelAnimation;
    }

    public override void Enter() {
        IdelFunctionality = IdelFunctionalityChooser(); ;
        counter = 0;
    }
    public override void UpdateState() {
        IdelFunctionality();
        float distance = Mathf.Abs(HelperFunctions.PointToDistance(unitVariables.transform.localPosition, startPos));
        if (distance>= walkBackDistance) {
            counter+=Time.deltaTime;
            if (counter > forgetTime) {
                needsRetreating = true;
                counter = 0;
            }
        } else if (distance <= 1f) {
            needsRetreating = false;
        }
        if (needsRetreating) {
            unitVariables.SetDirection((unitVariables.transform.localPosition.x - startPos.x > walkBackDistance) ? -1 : 1);
            IdelFunctionality = Walk;
            reset = true;
        } else {
            if (reset) {
                IdelFunctionality = IdelFunctionalityChooser();
                unitVariables.SetDirection(startDir);
                reset = false;
            }
        }
    }
    protected virtual void Stay() {
        rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
        animator.Play(unitVariables.animations.idelAnimation.name);
        
    }
    protected void Walk() {
        animator.Play(unitVariables.animations.runAnimation.name);
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * walkSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
    protected virtual IdelFunctionalityDelegate IdelFunctionalityChooser() {

        return Walk;
    }
}
