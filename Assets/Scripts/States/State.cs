using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
    private float totalTime = 0;
    private float time = 0;
    private float startValue = 0;
    private float endValue = 0;
    protected float newVelocity;
    protected float lerpedValue = 0;
    protected float oldVelocity = 0;
    private bool needsLerping = false;
    protected Rigidbody2D rb;
    protected Animator animator;
    public bool recovering { get; protected set; } = false;
    public bool interuptable { get; protected set; } = true;
    public bool stateDone;
    protected Unit unitVariables;
    public virtual void UpdateState () {
        if (needsLerping) {
            time += Time.deltaTime;
            float percentageDone = time / totalTime;
            if (percentageDone >= 1) {
                lerpedValue = Mathf.Lerp(startValue, endValue, percentageDone);
                needsLerping = false;
            }
            rb.linearVelocity = new Vector2(lerpedValue, rb.linearVelocity.y);
        }
    }
    public virtual void FixedUpdateState () { }
    public virtual void Enter () {
        if (rb.linearVelocity.x > oldVelocity) {
            oldVelocity = rb.linearVelocity.x;
            Lerp(oldVelocity, newVelocity, 1f);
        }
    }
    public virtual void Exit () {
        stateDone = true;
    }

    public void Setup (Rigidbody2D rb, Animator animator,Unit unitVariables) { 
        this.rb = rb;
        this.animator = animator;
        this.unitVariables = unitVariables;
    }
    public void ResetState (State oldState) {
        oldState.Exit();
        stateDone = false;
        Enter();
    }

    public void Lerp(float newStartValue, float newEndValue, float newTime) {
        needsLerping = true;
        startValue = newStartValue;
        endValue = newEndValue;
        totalTime = newTime;
        time = 0;
    }

}
