using UnityEngine;

public class AgroStateStay : AgroState
{
    [SerializeField] private float maxDistance;
    [SerializeField] protected AnimationClip idelAnimation;
    private Vector2 startPos;

    private void Start() {
        startPos = unitVariables.transform.position;
    }
    
    public override void UpdateState() {
        if (Mathf.Abs(HelperFunctions.PointToDistance(unitVariables.transform.position, startPos)) > maxDistance) {
            DistanceOver();
        } else {
            Run();
        }
    }
    protected virtual void DistanceOver() {
        animator.Play(idelAnimation.name);
        rb.linearVelocity = Vector2.zero;
    }

}
