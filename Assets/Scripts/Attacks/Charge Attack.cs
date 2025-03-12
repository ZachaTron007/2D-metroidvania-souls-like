using UnityEngine;

public class ChargeAttack : ParentMeleeAttack
{

    private RbVelocityLerp charge;
    [Header("Charge Settings")]
    [SerializeField] private float chargeSpeed = 5;
    [SerializeField] private float chargeTime = 2;
    [SerializeField] private float slowTime = .5f;
    [SerializeField] private AnimationCurve chargeCurve;

    protected override void Start() {
        base.Start();
        interuptable = .3f;
    }
    public override void Enter() {
        clip = unitVariables.animations.runAnimation;
        base.Enter();
        Attack();
    }
    private void Attack() {
        Debug.Log("Starting Charge");
        charge = new RbVelocityLerp(startSpeed: 0,chargeSpeed * unitVariables.GetDirection(), chargeTime, Vector2.right, rb, chargeCurve, FinishedLerpingEvent: SlowDown);
        SetHitBoxStatus(true);
    }

    public override void FixedUpdateState() {
        base.FixedUpdateState();
        charge.FixedUpdate();
    }
    private void SlowDown() {
        Debug.Log("Slowing Down");
        charge.ResetLerp(chargeSpeed * unitVariables.GetDirection(), 0, slowTime, Vector2.right, Exit);
    }

    public override void Exit() {
        base.Exit();
        unitVariables.isRecovering = true;
    }
}
