using Cinemachine;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class SwordEnemyScript : EnemyScript {
    //scrupts
    [Header("States")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private AgroState agroState;
    [SerializeField] private RecoveryState recoverState;
    [SerializeField] protected BaseIdelState idelState;
    [SerializeField] protected ParryRecoverState parryRecoverState;
    [SerializeField] private StunnedState stunnedState;
    [SerializeField] private MoveState moveState;

    private void Awake() {
        ComponentSetup();

        //attacks
        //state = idelState;
        moveState.Setup(rb, animatior, this, stun);
        idelState.Setup(rb, animatior, this, stun);
        agroState.Setup(rb, animatior,this, stun);
        recoverState.Setup(rb, animatior, this, stun);
        parryRecoverState.Setup(rb, animatior, this, stun);
        stunnedState.Setup(rb, animatior, this, stun);
        
        AgroAttackColliders();


    }
    private void Start() {
        state = idelState;
        state.Enter();
    }
    protected override void EventUnsubscribe() {
        base.EventUnsubscribe();
        stun.MaxValueReached -= ChangeToStunState;
        playerState.parried -= getParryed;
    }
    protected override void EventSubscribe() {
        base.EventSubscribe();
        playerState.parried+=getParryed;
        stun.MaxValueReached += ChangeToStunState;
    }
    // Update is called once per frame
    protected override void Update() {
        if (WallCheck(.3f)) {
            SetDirection(-GetDirection());
        }

        state.UpdateState();
        StateChange();
        directionFlip();
        
    }
    private void FixedUpdate() {
        state.FixedUpdateState();
    }

    public override void StateChange(State manualState = null) {
        State newState = state;
        newState = BehaviorController();
        if (manualState) {
            newState = manualState;
        }

        state = CanSwitchState(newState, state);

    }
    private State BehaviorController() {
        if (isWithinAgroRange) {
            return agroState;
        }

        if (isRecovering) {
            return recoverState;
        }

        if (isWithinAttackRange) {
            return attackState;
        }

        if (rb.linearVelocity.y < -.0001) {
            return fallState;
        }
        
        return idelState;
    }

    private void getParryed() {
        StateChange(parryRecoverState);
    }
    protected override void Die() {
        StateChange(dieState);
    }

    private void ChangeToStunState() {
        StateChange(stunnedState);
    }

}
