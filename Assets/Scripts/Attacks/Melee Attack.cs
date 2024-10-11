using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : State
{
    [SerializeField] protected AttackScript[] basicCombo;
    [SerializeField] private int attackNum = -1;
    [SerializeField] private AnimationClip currentClip;
    [SerializeField] private float currentClipTime;
    [SerializeField] protected AttackScript currentAttack;


    private Vector2 offset;
    public Vector2 lookDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        interuptable = false;
        offset.x = 0.87f;
        offset.y = 1.27f;
        //attackHitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public override void Enter() {
        if (unitVariables.attackTime >= currentClipTime) {
            UpdateAttack();
            
            StartCoroutine(Attack(unitVariables.direction));
            unitVariables.attackTime = 0.0f;
        } else {
            Exit();
        }

    }

    protected Vector2 offsetVector(float direction) {
        return new Vector2(direction*offset.x,offset.y);
    }

    public virtual IEnumerator Attack(float direction) {
        animator.Play(currentClip.name);
        float startAttack = currentClipTime / 2;
        yield return new WaitForSeconds(startAttack);
        currentAttack.attackHitBox.enabled = true;
        currentAttack.attackHitBox.offset = offsetVector(direction);
        float endAttack = currentClipTime / 2;
        yield return new WaitForSeconds(endAttack);
        currentAttack.attackHitBox.enabled = false;
        Exit();
    }
    public override void Exit() {
        
        
        stateDone = true;
    }
    public override void UpdateState() {
        
        //attackTime += Time.deltaTime;
    }

    public void UpdateAttack() {
        //makes attack num go up
        attackNum++;
        //resets attackNum to be withijn the combo
        float comboEndTime = currentClipTime + .5f;
        if (attackNum >= basicCombo.Length) {
            attackNum = 0;
        }
        if (unitVariables.attackTime >= comboEndTime) {
            attackNum = 0;
        }
        //sets the current attack
        currentAttack = basicCombo[attackNum];
        currentClip = currentAttack.clip;
        currentClipTime = currentAttack.length;
    }
}
