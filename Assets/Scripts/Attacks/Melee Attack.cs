using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : State
{
    [SerializeField] private AnimationClip[] basicComboClip;
    private BoxCollider2D attackHitBox;
    [SerializeField] private float attackTime;
    [SerializeField] private float knockback = 5f;
    [SerializeField] private int attackNum;

    private Vector2 offset;
    public Vector2 lookDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        interuptable = false;
        offset.x = 0.87f;
        offset.y = 1.27f;
        attackHitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public override void Enter() {
        if (attackNum == 3) {
            attackNum = 0;
        }
        float comboEndTime = basicComboClip[attackNum].length + .5f;
        if (playerVariables.attackTime >= comboEndTime) {
            attackNum = 0;
        }
        if (playerVariables.attackTime >= basicComboClip[attackNum].length) {
            StartCoroutine(Attack(playerVariables.direction));
            playerVariables.attackTime = 0.0f;
        } else {
            Exit();
        }

    }

    private Vector2 offsetVector(float direction) {
        return new Vector2(direction*offset.x,offset.y);
    }

    public IEnumerator Attack(float direction) {
        animator.Play(basicComboClip[attackNum].name);
        float startAttack = basicComboClip[attackNum].length/2;
        yield return new WaitForSeconds(startAttack);
        attackHitBox.enabled = true;
        attackHitBox.offset = offsetVector(direction);
        float endAttack = basicComboClip[attackNum].length / 2;
        yield return new WaitForSeconds(endAttack);
        attackHitBox.enabled = false;
        Exit();
    }
    public override void Exit() {
        attackNum++;
        stateDone = true;
    }
    public override void UpdateState() {
        //attackTime += Time.deltaTime;
    }

}
