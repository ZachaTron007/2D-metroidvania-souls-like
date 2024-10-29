using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockState : State
{
    [SerializeField] private AnimationClip blockingClip;
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] private float parryWindow = 5f;
    
    [SerializeField] private bool blocking = false;
    [SerializeField] private bool parrying = false;
    public event Action <bool> onBlock;


    public override void Enter() {
        Block();
        interuptable = false;

    }

    public override void UpdateState() {
        if (Input.GetMouseButtonUp(1)||!Input.GetMouseButton(1)) {
            Exit();
        }
    }

    public override void Exit() {
        onBlock?.Invoke(false);
        stateDone = true;
    }

    public void Block() {
        onBlock?.Invoke(true);
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
