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
        unStunTimer = 0;
        Debug.Log(stunClip.name);
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
        base.Exit();
    }
}
