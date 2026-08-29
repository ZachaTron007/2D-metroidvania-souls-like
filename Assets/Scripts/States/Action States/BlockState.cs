using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class BlockState : State
{
    [SerializeField] public float parryWindow = 5f;
    
    [SerializeField] private bool blocking = false;
    [SerializeField] public float parryCounter;
    public bool canParry;
    //[SerializeField] Health health;
    protected void Start() {
        clip = unitVariables.animations.blockAnimation;
    }
    public override void Enter() {
        base.Enter();
        rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
        canParry = true;
        parryCounter = 0;
        blocking = true;
    interuptable = .2f;

    }

    public override void UpdateState() {
        base.UpdateState();
        parryCounter += Time.deltaTime;
        if (parryCounter > parryWindow) {
            canParry = false;
        }
        if (Input.GetMouseButtonUp(1)||!Input.GetMouseButton(1)) {
            StateIsDone();
        }
    }

    protected override void StateIsDone() {
        base.StateIsDone();
        blocking = false;
        canParry = false;
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
