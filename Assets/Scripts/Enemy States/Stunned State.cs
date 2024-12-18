using UnityEngine;
using UnityEngine.InputSystem.Android;

public class StunnedState : State
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AnimationClip stunClip;
    private float unStunTimer;
    private float unStunTime = 3f;
    void Awake()
    {
        interuptable = .8f;
    }

    public override void Enter() {
        stun.canChange = false;
        stun.SetCurrentValue(0);
        unStunTimer = 0;
        animator.Play(stunClip.name);
    }

    public override void UpdateState() {
        base.UpdateState();
        if (unStunTimer > unStunTime) {
            Exit();
        } else {
            unStunTimer += Time.deltaTime;
        }
    }
    public override void Exit() {
        stun.canChange = true;
        base.Exit();
    }
}
