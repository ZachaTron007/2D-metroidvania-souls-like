using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
    protected Rigidbody2D rb;
    protected float direction;
    protected Animator animator;
    public bool recover { get; protected set; } = false;
    public bool interuptable { get; protected set; } = true;
    public bool stateDone;
    protected Unit playerVariables;
    public virtual void UpdateState () { }
    public virtual void FixedUpdateState () { }
    public virtual void Enter () { }
    public virtual void Exit () { }

    public void Setup (Rigidbody2D rb, Animator animator, PlayerState playerVariables=null,Unit playerVariable=null) { 
        this.rb = rb;
        this.animator = animator;
        if (playerVariables) {
            this.playerVariables = playerVariables;
        }else if (playerVariable) {
            this.playerVariables = playerVariable;
        }
    }
    public void ResetState () {
        stateDone = false;
        Enter();
    }
    
}
