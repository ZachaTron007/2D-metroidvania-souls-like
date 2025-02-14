using UnityEngine;

public class RbVelocityLerp
{
    [Header("Lerp Settings")]
    [SerializeField] protected float totalTime = 1;
    private Vector2 dir;
    public float startSpeed;
    public Vector2 movement;
    public float targetSpeed;
    [SerializeField] private float counter;
    [SerializeField] private AnimationCurve curve;
    private LerpingInhertedClass lerpClass;
    private Rigidbody2D rb;
    public RbVelocityLerp(float startSpeed, float targetSpeed, float totalTime, Vector2 dir, Rigidbody2D rb, AnimationCurve curve, LerpingInhertedClass lerpClass = null) {
        ResetLerp(startSpeed, targetSpeed, totalTime, dir);
        this.rb = rb;
        this.lerpClass = lerpClass;
        this.curve = curve;
    }
    public void ResetLerp(float startSpeed, float targetSpeed, float totalTime, Vector2 dir) {
        this.startSpeed = startSpeed;
        this.totalTime = totalTime;
        counter = totalTime;
        this.dir = dir;
        ChangeTargetSpeed(targetSpeed);

    }
    public void ChangeTargetSpeed(float targetSpeed) {
        this.targetSpeed = targetSpeed;
    }

    public void FixedUpdate() {
        movement = HelperFunctions.LerpHelper(startSpeed, targetSpeed, totalTime, ref counter, curve) * dir;
        movement-= rb.linearVelocity * new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        rb.linearVelocity += movement;
        if (counter <= 0) { FinishedLerping(); }
    }
    protected void FinishedLerping() {
        lerpClass?.FinishedLerping();
    }
}
