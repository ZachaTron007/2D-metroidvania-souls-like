using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockState : State
{
    [SerializeField] private AnimationClip blockingClip;
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] public float parryWindow = 5f;
    
    [SerializeField] private bool blocking = false;
    [SerializeField] public float parryCounter;
    public bool canParry;
    //[SerializeField] Health health;


    public override void Enter() {
        rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
        canParry = true;
        parryCounter = 0;
        Block();
        interuptable = .5f;

    }

    public override void UpdateState() {
        parryCounter += Time.deltaTime;
        if (parryCounter > parryWindow) {
            canParry = false;
        }
        if (Input.GetMouseButtonUp(1)||!Input.GetMouseButton(1)) {
            Exit();
        }
    }

    public override void Exit() {
        blocking = false;
        stateDone = true;
        canParry = false;
    }

    public void Block() {
        animator.Play(blockingClip.name);
        //float parryWindow = .5f;
        blocking = true;
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
