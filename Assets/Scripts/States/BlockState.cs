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
    [SerializeField] private float parryCounter;
    public event Action <bool> onBlock;


    public override void Enter() {
        parryCounter = 0;
        Block();

        interuptable = false;

    }

    public override void UpdateState() {
        parryCounter += Time.deltaTime;
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
        blocking = true;

        /*
        if (Input.GetKeyDown(KeyCode.B)) {
            animatior.SetTrigger(BLOCKPARRY);
        }
        */
    }
    public bool? IsBlockingAttack(AttackInfo enemyAttack) {
        if (blocking) {
            int directionOfEnemy = enemyAttack.attackHitBox.offset.x>0?1:-1;
            if (unitVariables.GetDirection() != directionOfEnemy) {
                return true;    
            }
        }
        return false;
    }
}
