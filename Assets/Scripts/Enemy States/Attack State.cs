using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : ParentMeleeAttack
{

    private bool attacking;
    public AttackScript[] attacks;
    [SerializeField] private AttackScript attack;

    /*
     * summary:
     *  1. chooses a random attack out of all the possible attacks
     *  2. starts the attack function
     *  */
    public override void Enter() {

        interuptable = false;
        currentAttack = randomAttackPicker(attacks);
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(Attack());
    }
    
    
    public override void Exit() {
        stateDone = true;
        unitVariables.isRecovering = true;
    }

    private AttackScript randomAttackPicker(AttackScript[] attacks) {
        int attackNum = Random.Range(0, attacks.Length);
        return attacks[attackNum];
    }
}