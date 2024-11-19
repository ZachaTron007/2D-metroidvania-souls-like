using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdelState : BaseIdelState {
    // Start is called before the first frame update
    //movements
    
    private float speed;
    private bool idel = false;

    // Update is called once per frame
    public override void Enter() {
        idel = true;
        Walk();
        
    }
    private void Stay() {
        if (idel) {
            animator.Play(idelAniamtion.name);
            speed = 0;
            float stopLowSpeed = 1f;
            float stopHighSpeed = 2f;
            Invoke("Walk", Random.Range(stopLowSpeed, stopHighSpeed));
        }
    }
    private void Walk() {
        if (idel) {
            animator.Play(walkAnimation.name);
            speed = walkSpeed;
            float stopLowSpeed = 1f;
            float stopHighSpeed = 2f;
            Invoke("Stay", Random.Range(stopLowSpeed, stopHighSpeed));
        }
    }
    
    public override void FixedUpdateState() {
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * speed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }
    public override void UpdateState() {
        base.UpdateState();
    }
    public override void Exit() {
        idel = false;
        rb.linearVelocity = Vector2.zero;
    }
    
}
