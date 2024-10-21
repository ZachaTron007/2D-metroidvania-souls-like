using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdelState : State {
    // Start is called before the first frame update
    //movements
    [SerializeField] private AnimationClip idelAniamtion;
    [SerializeField] private AnimationClip walkAniamtion;
    [SerializeField] private int idelWalkSpeed = 50;
    [SerializeField] delegate void idelDelegate();
    [SerializeField] idelDelegate idelState;
    private bool idel = false;

    // Update is called once per frame
    public override void Enter() {
        idel = true;
        idelState = Walk;
        idelState();



    }
    private void Stay() {
        if (idel) {
            animator.Play(idelAniamtion.name);
            rb.linearVelocity = Vector2.zero;
            float stopLowSpeed = 1f;
            float stopHighSpeed = 2f;
            Invoke("Walk", Random.Range(stopLowSpeed, stopHighSpeed));
        }
    }
    private void Walk() {
        if (idel) {
            animator.Play(walkAniamtion.name);
            rb.linearVelocity = new Vector2(unitVariables.direction * idelWalkSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
            float stopLowSpeed = 1f;
            float stopHighSpeed = 2f;
            Invoke("Stay", Random.Range(stopLowSpeed, stopHighSpeed));
        }
    }
    public override void Exit() {
        idel = false;
        rb.linearVelocity = Vector2.zero;
    }
    
}
