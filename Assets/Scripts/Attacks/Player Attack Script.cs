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
    private void Awake() {
        currentAttack = basicCombo[0];
    }
    public override void Enter() {
        base.Enter();
        UpdateAttack();
        unitVariables.attackTime = 0;
        StartCoroutine(attack);
        unitVariables.attackTime = 0;/*
        if (unitVariables.attackTime >= currentClipTime) {
            
            
            
        } else {
            Exit();
        }*/

    }

    public override void Exit() {
        base.Exit();
        unitVariables.attackTime = 0;
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
