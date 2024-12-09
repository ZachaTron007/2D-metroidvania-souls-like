using Cinemachine;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordEnemyScript : EnemyScript {
    //scrupts
    [Header("States")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private AgroState agroState;
    [SerializeField] private RecoveryState recoverState;
    [SerializeField] protected BaseIdelState idelState;
    [SerializeField] protected ParryRecoverState parryRecoverState;
    [SerializeField] private StunnedState stunnedState;

    private void Awake() {
        ComponentSetup();

        //attacks
        //state = idelState;
        idelState.Setup(rb, animatior, this);
        agroState.Setup(rb, animatior,this);
        recoverState.Setup(rb, animatior, this);
        parryRecoverState.Setup(rb, animatior, this);
        stunnedState.Setup(rb, animatior, this);
        state = idelState;
        state.Enter();
        AgroAttackColliders();


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
    void Update() {
        if (WallCheck()) {
            SetDirection(-GetDirection());
        }

        state.UpdateState();
        //InteruptrableStateChange();
        StateChange();
        directionFlip();
        
    }
    private void FixedUpdate() {
        state.FixedUpdateState();
    }

    protected override void StateChange(State manualState = null) {
        State newState = state;
        if (!isWithinAgroRange) {
            newState = idelState;
        } else {
            if (isRecovering) {
                newState = recoverState; 
            } 
            else if (!isWithinAttackRange) {
                newState = agroState;
            } else {
                newState = attackState;
            }

        }
        if (manualState) {
            newState = manualState;
        }
        state = CanSwitchState(newState);

    }

    private void getParryed() {
        StateChange(parryRecoverState);
    }
    protected override void Die() {
        StateChange(dieState);
    }

    private void ChangeToStunState() {
        Debug.Log("Switching");
        StateChange(stunnedState);
    }

}
