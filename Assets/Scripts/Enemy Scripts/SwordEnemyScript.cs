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

    private void Awake() {
        ComponentSetup();

        //attacks
        //state = idelState;
        idelState.Setup(rb, animatior, this);
        agroState.Setup(rb, animatior,this);
        recoverState.Setup(rb, animatior, this);
        parryRecoverState.Setup(rb, animatior, this);
        state = idelState;
        state.Enter();
        AgroAttackColliders();

    }
    protected override void EventSubscribe() {
        playerState.parried+=getParryed;
    }
    // Update is called once per frame
    void Update() {
        if (WallCheck()) {
            direction *= -1;
            switchHitboxDirection(direction);
        }

        state.UpdateState();
        InteruptrableStateChange();
        directionFlip();
        
    }
    private void FixedUpdate() {
        state.FixedUpdateState();
    }

    protected override void StateChange(State manualState = null) {

        State oldState = state;
        if (!isWithinAgroRange) {
            state = idelState;
        } else {
            if (!isWithinAttackRange) {
                state = agroState;
            } else {
                if (!isRecovering) {
                    state = attackState;
                } else {
                    state = recoverState;
                }
            }

        }
        if (manualState) {
            state=manualState;
            state.ResetState(oldState);
            return;
        }
        if (oldState != state) {
            state.ResetState(oldState);
        }

    }

    private void getParryed() {
        Debug.Log("Was Parried");
        StateChange(parryRecoverState);
    }
    protected override void Die() {
        StateChange(dieState);
    }

}
