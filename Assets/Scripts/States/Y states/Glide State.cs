using UnityEngine;
using UnityEngine.Rendering;

public class GlideState : State
{
    [SerializeField] private float fallSpeed = -50f;
    private float transitionTime;
    [SerializeField] private float totalTransitionTime = 1;
    private bool inDraft;
    private float goalSpeed;
    private float startFallSpeed;
    private DraftScript currentDraft;
    protected void Start() {
        clip = unitVariables.animations.fallAniamtion;
    }
    public override void Enter() {
        base.Enter();
        interuptable = .1f;
        unitVariables.mainCollider.triggerStay += triggerDetectionStay;
        unitVariables.mainCollider.triggerExit += triggerDetectionExit;
        ChangeSpeed(fallSpeed);
        rb.gravityScale = 0;
    }
    public override void UpdateState() {
        base.UpdateState();
        if (!Input.GetKey(KeyCode.Space)) {
            currentDraft = null;
            StateIsDone();
        }

        if (transitionTime>0) {
            transitionTime -= Time.deltaTime;
            if (transitionTime <= 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, fallSpeed * Time.deltaTime);
                
            }
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Lerp(startFallSpeed, goalSpeed, 1-(transitionTime/totalTransitionTime)));
        }
    }
    private void ChangeSpeed(float endSpeed) {
        transitionTime = totalTransitionTime;
        goalSpeed = endSpeed;
        startFallSpeed = rb.linearVelocityY;
    }
    protected override void StateIsDone() {
        base.StateIsDone();
        unitVariables.mainCollider.triggerStay -= triggerDetectionStay;
        unitVariables.mainCollider.triggerExit -= triggerDetectionExit;
        rb.gravityScale = 2f;
    }
    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<DraftScript>() != null) {
            rb.gravityScale = fallSpeed;
        }
    }
    public override void triggerDetectionStay(Collider2D collider) {
        if (!currentDraft) {
            if (collider.gameObject.GetComponent<DraftScript>() != null) {
                currentDraft = collider.gameObject.GetComponent<DraftScript>();
                inDraft = true;
            }
        } else if(inDraft){
            ChangeSpeed(currentDraft.draftSpeed);
            inDraft = false;
        }
    }
    public override void triggerDetectionExit(Collider2D collider) {
        base.triggerDetectionEnter(collider);
        if (collider.gameObject.GetComponent<DraftScript>() != null) {
            ChangeSpeed(fallSpeed);
            currentDraft = null;
        }
    }

}
