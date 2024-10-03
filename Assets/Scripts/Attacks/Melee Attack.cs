using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : State
{
    [SerializeField] private AttackScript[] basicCombo;
    [SerializeField] private int attackNum = -1;
    [SerializeField] private AnimationClip currentClip;
    [SerializeField] private float currentClipTime;
    [SerializeField] AttackScript currentAttack;


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
        float comboEndTime = currentClipTime + 5f;
        
        if (playerVariables.attackTime >= currentClipTime) {
            UpdateAttack();
            if (playerVariables.attackTime >= comboEndTime) {
                attackNum = 0;
            }
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
        if (attackNum >= basicCombo.Length) {
            attackNum = 0;
        }
        //sets the current attack
        currentAttack = basicCombo[attackNum];
        currentClip = currentAttack.clip;
        currentClipTime = currentAttack.length;
    }
}
