using UnityEngine;
using UnityEngine.InputSystem.Android;

public class RepositionState : RecoveryState {
    [SerializeField] private AnimationClip walkAniamtion;
    [SerializeField] private float speed = 50;
    private delegate void WaitingDelegate(int direction);
    private WaitingDelegate waitingDelegate;
    private float counter;
    [SerializeField] private float switchTime;
    [SerializeField] private float range = 2;
    [SerializeField] private float point;
    public override void Enter() {
        base.Enter();
        interuptable = false;
        waitingDelegate = MoveAround;
        point = gameObject.transform.position.x + range;
        waitingDelegate(-unitVariables.GetDirection());
    }

    private void MoveAround(int direction) {
        animator.Play(walkAniamtion.name);
        rb.linearVelocity = Vector2.right * direction * speed;
    }
    private void Stay(int direction) {
        animator.Play(idelAniamtion.name);
        rb.linearVelocity = Vector2.zero;
    }
    public override void UpdateState() {
        if(gameObject.transform.position.x > point) {
            MoveAround(unitVariables.GetDirection());
        }
    }/*
    private void Stop(int direction) {
        rb.linearVelocity = Vector2.right * direction * 0;
    }
    public override void Exit() {
        rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState() {
        Debug.Log(waitingDelegate.Method);
        waitingDelegate(-(int)direction);
        counter += Time.deltaTime;
        if (counter > switchTime) {
            if (waitingDelegate == MoveAround) {
                waitingDelegate = Stop;
                
            } else {
                waitingDelegate = MoveAround;
                direction *= -1;
            }
            counter = 0;
        }

    }8*/
}

