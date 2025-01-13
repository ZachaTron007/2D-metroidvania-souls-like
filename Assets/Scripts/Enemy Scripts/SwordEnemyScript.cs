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
    void Update() {
        if (WallCheck(.3f)) {
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
        
        if (rb.linearVelocity.y < -.0001) {
            newState = fallState;
        }
        if (manualState) {
            newState = manualState;
        }

        state = CanSwitchState(newState, state);

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
