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
    [SerializeField] private IdelState idelState;
    [SerializeField] private AgroState agroState;
    [SerializeField] private EnemyAttackState attackState;
    [SerializeField] private RecoveryState recoverState;


    private void Awake() {
        ComponentSetup();

        //attacks
        //state = idelState;
        idelState.Setup(rb, animatior,UnitVariables: this);
        agroState.Setup(rb, animatior, UnitVariables: this);
        attackState.Setup(rb, animatior, UnitVariables: this);
        recoverState.Setup(rb, animatior, UnitVariables: this);
        state = idelState;
        state.Enter();
        AgroAttackColliders();

    }

    

    // Update is called once per frame
    void Update() {
        direction = WallCheck();

        state.UpdateState();
        InteruptrableStateChange();
        directionFlip();
        
    }
    private void FixedUpdate() {
        state.FixedUpdateState();
    }

    protected override void StateChange() {
        State oldState = state;
        if (!isWithinAgroRange) {
            state = idelState;
        } else {
            if (!isWithinAttackRange) {
                state = agroState;
            } else {
                if (!state.recovering) {
                    state = attackState;
                } else {
                    state = recoverState;
                }
            }

        }
        
        if (oldState != state) {
            state.ResetState(oldState);
        }
    }

}
