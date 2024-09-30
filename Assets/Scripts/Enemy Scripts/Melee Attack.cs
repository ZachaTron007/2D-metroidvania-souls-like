using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : State
{
    [SerializeField] private AnimationClip AttackClip;
    private BoxCollider2D attackHitBox;
    [SerializeField] private float attackTime = .25f;
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
        animator.Play(AttackClip.name);
        if (attackNum==3) {
            attackNum = 0;
        }
        attackNum++;
        StartCoroutine(Attack(playerVariables.direction));
    }

    private Vector2 offsetVector(float direction) {
        Vector2 dir = new Vector2(direction*offset.x,offset.y);
        return dir;
    }

    public IEnumerator Attack(float direction) {
        float startAttack = .15f;
        yield return new WaitForSeconds(startAttack);
        attackHitBox.enabled = true;
        attackHitBox.offset = offsetVector(direction);
        float endAttack = .15f;
        yield return new WaitForSeconds(endAttack);
        attackHitBox.enabled = false;
        Exit();
    }
    public override void Exit() {
        stateDone = true;
    }

}
