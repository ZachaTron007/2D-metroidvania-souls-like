using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class State : MonoBehaviour {
    protected Rigidbody2D rb;
    protected Animator animator;
    public bool recovering { get; protected set; } = false;
    public float interuptable = 0;//{ get; protected set; } = 0;
    public bool stateDone;
    protected Unit unitVariables;
    protected void Start() {
        if (!animator || !rb || !unitVariables) {
            Debug.Log("YOU HAVENT CALLED SETUP ON " + gameObject.name + "!!!, ON UNIT: "+transform.root.name);
        }
    }
    public virtual void UpdateState () {

    }
    public virtual void FixedUpdateState () { }
    public virtual void Enter () {
    }
    public virtual void Exit () {
        stateDone = true;
    }

    public void Setup (Rigidbody2D rb, Animator animator,Unit unitVariables) { 
        this.rb = rb;
        this.animator = animator;
        this.unitVariables = unitVariables;
    }
    public void ResetState (State newState) {
        Exit();
        newState.stateDone = false;
        newState.Enter();
    }

}
