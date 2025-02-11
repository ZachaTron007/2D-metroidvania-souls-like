using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : ParentMeleeAttack {
    [SerializeField] protected AttackInfo[] basicCombo;
    [SerializeField] private int attackNum = -1;
    [SerializeField] private AnimationClip currentClip;
    public float currentClipTime;
    [SerializeField] private float comboEndTime = .2f;
    


    // Update is called once per frame
    private void Awake() {
        currentAttack = basicCombo[0];
        canInteruptSelf = true;
    }
    public override void Enter() {
        base.Enter();
        
        UpdateAttack();
        StopCoroutine(attack);
        StartCoroutine(attack);

    }

    public override void Exit() {
        base.Exit();
        attackTime = 0;
    }
    public void Update() {

        attackTime += Time.deltaTime;
    }

    public void UpdateAttack() {
        //makes attack num go up
            attackNum++;
        //resets attackNum to be withijn the combo
        float comboEndTime = currentClipTime + this.comboEndTime;
        if (attackNum >= basicCombo.Length) {
            attackNum = 0;
        }
        if (attackTime >= comboEndTime) {
            attackNum = 0;
        }
        //sets the current attack
        currentAttack = basicCombo[attackNum];
        currentClip = currentAttack.clip;
        currentClipTime = currentAttack.length * (1/attackSpeedModifier);
    }
}
