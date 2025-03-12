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
    [SerializeField] private List<ParentMeleeAttack> attacks = new List<ParentMeleeAttack>{};

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
        for (int i = 0; i < attacks.Count; i++) {
            attacks[i].Setup(rb, animatior, this, stun);
        }
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
        //InteruptrableStateChange();
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
        
        Debug.Log(isRecovering);
        if (isRecovering) {
            return recoverState;
        }
        Debug.Log("Should be attacking: "+isWithinAttackRange);
        if (isWithinAttackRange) {
            Debug.Log("Should be charging");
            return attacks[2];
            return attacks[UnityEngine.Random.Range(0,attacks.Count-1)];
        }
        if (isWithinAgroRange) {
            return agroState;
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
