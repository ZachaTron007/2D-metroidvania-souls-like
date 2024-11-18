using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : ParentMeleeAttack
{
    private Sensors attackHitBox;
    private bool attacking;
    public AttackInfo[] attacks;
    private AttackInfo[] possibleAttacks;

    /*
     * summary:
     *  1. chooses a random attack out of all the possible attacks
     *  2. starts the attack function
     *  */
    public override void Enter() {
        base.Enter();
        interuptable = false;
        currentAttack = randomAttackPicker(attacks);
        
        StartCoroutine(attack);
    }
    public override void FixedUpdateState() {
        //rb.linearVelocity = Vector2.zero;
    }

    public override void Exit() {
        base.Exit();
        
        unitVariables.isRecovering = true;
    }

    private AttackInfo randomAttackPicker(AttackInfo[] attacks) {
        List<AttackInfo> list = new List<AttackInfo>();
        
        possibleAttacks = new AttackInfo[0];
        for (int i = 0; i < attacks.Length; i++) {
            list.Add(attacks[i]);
            Debug.Log(list[i].name);
        }
        Debug.Log(list.Count);
        Debug.Log(list.Capacity);
        int attackNum = Random.Range(0, list.Count);
        return list[attackNum];
    }
}