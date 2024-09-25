using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
    protected Rigidbody2D rb;
    protected float direction;
    protected Animator animator;
    protected bool interuptable = true;
    //public bool stateDone = false;
    protected Temp playerVariables;
    public virtual void UpdateState () { }
    public virtual void FixedUpdateState () { }
    public virtual void Enter () { }
    public virtual void Exit () { }

    public void Setup (Rigidbody2D rb, Animator animator,Temp playerVariables) { 
        this.rb = rb;
        this.animator = animator;
        this.playerVariables = playerVariables;
    }
    
}
