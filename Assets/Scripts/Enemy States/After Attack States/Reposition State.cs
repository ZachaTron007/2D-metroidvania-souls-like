using UnityEngine;
using UnityEngine.InputSystem.Android;

public class RepositionState : RecoveryState {
    [SerializeField] private AnimationClip walkAniamtion;
    [SerializeField] private float disengageSpeed = 50;
    [SerializeField] private float engageSpeed = 50;
    private delegate void WaitingDelegate(int direction,float speed);
    private WaitingDelegate waitingDelegate;
    private float counter;
    [SerializeField] private float switchTime;
    [SerializeField] private float range = 2;
    [SerializeField] private float point;
    public override void Enter() {
        //base.Enter();
        interuptable = false;
        waitingDelegate = MoveAround;
        point = unitVariables.transform.position.x + range*-unitVariables.GetDirection();
        Debug.Log("Point: "+point);
        Debug.Log("Transform: " + unitVariables.transform.position.x);
    }

    private void MoveAround(int direction,float speed) {
        animator.Play(walkAniamtion.name);
        rb.linearVelocity = Vector2.right * direction * speed*Time.fixedDeltaTime;
    }
    private void Stay(int direction) {
        animator.Play(idelAniamtion.name);
        rb.linearVelocity = Vector2.zero;
        Exit();
    }
    public override void FixedUpdateState() {
        if(unitVariables.transform.position.x > point) {
            MoveAround(-unitVariables.GetDirection(),disengageSpeed);
        } else {
            Stay(unitVariables.GetDirection());
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

