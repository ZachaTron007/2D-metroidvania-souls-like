using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : ParentMeleeAttack {
    [SerializeField] protected AttackInfo[] basicCombo;
    [SerializeField] private int attackNum = -1;
    [SerializeField] private AnimationClip currentClip;
    [SerializeField] private float currentClipTime;


    // Update is called once per frame
    public override void Enter() {
        if (unitVariables.attackTime >= currentClipTime) {
            unitVariables.attackTime = 0.0f;
            UpdateAttack();
            
            StartCoroutine(Attack());
            
        } else {
            Exit();
        }

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
