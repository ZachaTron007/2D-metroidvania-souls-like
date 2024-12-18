using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : ParentMeleeAttack
{
    private Sensors attackHitBox;
    private bool attacking;
    public AttackInfo[] attacks;
    private AttackInfo[] possibleAttacks;
    private List<AttackInfo> list = new List<AttackInfo>();
    /*
     * summary:
     *  1. chooses a random attack out of all the possible attacks
     *  2. starts the attack function
     *  */
    public override void Enter() {
        base.Enter();
        interuptable = 1f;
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
        
        
        possibleAttacks = new AttackInfo[0];
        for (int i = 0; i < attacks.Length; i++) {
            list.Add(attacks[i]);
            if (attacks[i].gameObject?.GetComponent<Sensors>()) {
                Sensors sensor = attacks[i].gameObject?.GetComponent<Sensors>();
                
            }
        }
        int attackNum = Random.Range(0, list.Count);
        return list[attackNum];
    }

    private void TriggerDection(Collider2D collider) {
        list.Add(collider.gameObject.GetComponent<AttackInfo>());
        Debug.Log("Added Attack");
    }

}