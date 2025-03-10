using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class State : MonoBehaviour {
    protected Rigidbody2D rb;
    protected Animator animator;
    protected AnimationClip clip;
    public bool stateDone = false;
    [field: Header("State Properties")]
    [field: SerializeField] public float interuptable { get; protected set; } = 0;
    [field: SerializeField] public bool canInteruptSelf { get; protected set; } = false;
    protected Unit unitVariables;
    protected Stun stun;
    public bool IsStateDone() {
        return stateDone;
    }
    protected virtual void StateIsDone() {
        stateDone = (stateDone)? false : true ;
    }
    public virtual void UpdateState () {

    }
    public virtual void FixedUpdateState () { }
    public virtual void Enter () {
        stateDone = false;
        unitVariables.animationList.AddLast(clip);
        animator.Play(unitVariables.animationList.Last.Value.name);

        
    }
    public virtual void Exit () {
        stateDone = true;
        unitVariables.animationList.Remove(clip);
        if (unitVariables.animationList.Count > 0) animator.Play(unitVariables.animationList.Last.Value?.name);

    }

    public void Setup (Rigidbody2D rb, Animator animator, Unit unitVariables,Stun stun = null) { 
        this.rb = rb;
        this.animator = animator;
        this.unitVariables = unitVariables;
        this.stun = stun;

    }
    public void ResetState (State newState, bool hasOldState = true) {
        if(hasOldState) Exit();
        newState?.Enter();
    }

    public virtual void collisionDetectionEnter(Collision2D collider) { }
    public virtual void triggerDetectionEnter(Collider2D collider) {
    }
    public virtual void collisionDetectionExit(Collision2D collider) { }
    public virtual void triggerDetectionExit(Collider2D collider) { }
    public virtual void triggerDetectionStay(Collider2D collider) { }

}
