using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    private bool attacking;
    public AttackScript[] attacks;
    [SerializeField] private AttackScript currentAttack;
    [SerializeField] private AttackScript attack;

    public override void Enter() {

        /*
        Debug.Log(attacks);
        int attackNum = Random.Range(0, attacks.Length - 1);
        Debug.Log("The Chosen Attack Number: "+attackNum);
        Debug.Log("The Length of the possible attacks: " + attacks.Length);
        Debug.Log("The first Attack of basic: " + basicCombo[0]);
        Debug.Log("The first Attack: "+ attacks[0]);
        Debug.Log("The chosen Attack:"+attacks[attackNum]);*/
        currentAttack = attack;
        Debug.Log(currentAttack.name);
        rb.velocity = Vector2.zero;
        StartCoroutine(Attack(direction));
    }

    private void Attack() {
        attacking = true;
        attacking = false;
    }

    public IEnumerator Attack(float direction) {
        animator.Play(currentAttack.clip.name);
        float startAttack = currentAttack.clip.length / 2;
        yield return new WaitForSeconds(startAttack);
        currentAttack.attackHitBox.enabled = true;
        currentAttack.attackHitBox.offset = offsetVector(direction);
        float endAttack = currentAttack.clip.length / 2;
        yield return new WaitForSeconds(endAttack);
        currentAttack.attackHitBox.enabled = false;
        Exit();
    }
    public override void Exit() {
        stateDone = true;
    }
    protected Vector2 offsetVector(float direction) {
        return new Vector2(direction * .87f, 1.27f);
    }
}
