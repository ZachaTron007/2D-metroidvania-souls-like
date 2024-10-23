using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : ParentMeleeAttack
{

    private bool attacking;
    public AttackInfo[] attacks;
    [SerializeField] private AttackInfo attack;

    /*
     * summary:
     *  1. chooses a random attack out of all the possible attacks
     *  2. starts the attack function
     *  */
    public override void Enter() {

        interuptable = false;
        currentAttack = randomAttackPicker(attacks);
        
        StartCoroutine(Attack());
    }
    public override void FixedUpdateState() {
        rb.linearVelocity = Vector2.zero;
    }

    public override void Exit() {
        stateDone = true;
        unitVariables.isRecovering = true;
    }

    private AttackInfo randomAttackPicker(AttackInfo[] attacks) {
        int attackNum = Random.Range(0, attacks.Length);
        return attacks[attackNum];
    }
}