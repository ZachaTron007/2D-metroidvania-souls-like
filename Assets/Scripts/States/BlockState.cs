using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockState : State
{
    [SerializeField] private Health1 health;
    [SerializeField] private AnimationClip blockingClip;
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] private float parryWindow = 5f;
    
    [SerializeField] private bool blocking = false;
    [SerializeField] private bool parrying = false;


    public override void Enter() {
        Block();
        interuptable = false;
        health.blocking = true;

    }

    public override void UpdateState() {
        if (Input.GetMouseButtonUp(playerVariables.blockButton)) {
            Exit();
        }
    }

    public override void Exit() {
        health.blocking = false;
        stateDone = true;
    }

    public void Block() {
        animator.Play(blockingClip.name);
        //float parryWindow = .5f;
        parrying = true;
        Invoke("ParryWindowEnd", parryWindow);
        blocking = true;

        /*
        if (Input.GetKeyDown(KeyCode.B)) {
            animatior.SetTrigger(BLOCKPARRY);
        }
        */
    }
    private void ParryWindowEnd() {
        parrying = false;
    }
}
