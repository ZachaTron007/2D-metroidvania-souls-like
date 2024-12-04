using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;


public class Health : ReducableStats {

    [SerializeField] private BlockState blockState;
    private Vector4 hurtColor = new Vector4(255, 62, 62, 255);
    private Vector4 normalColor;
    private float colorChangeSpeed = 0.2f;
    private SpriteRenderer sr;
    [SerializeField] private Sensors hitBox;
    //[SerializeField] private float parryWindow = 5f;
    public event Action<bool, DamageScript> hitEvent;
    public event Action dieEvent;
    private Unit Unit;
    private int damageAmount = 0;

    // Update is called once per frame

    private void Awake() {
        currentValue = MAX_VALUE;
        Unit = GetComponent<Unit>();
        sr = Unit.sr;
        normalColor = sr.color;
        hurtColor /= 255;
        hitBox.triggerEnter += GetHit;
    }

    void Update() {
        if (currentValue == 0) {
            SetCurrentValue(-1);
            die();
        }
    }

    public void Damage(int damage) {
        if (damage > 0) {
            ChangeCurrentValue(-damage);
            if (currentValue < 0) {
                SetCurrentValue(0);
            }
            StartCoroutine(HurtColorShift(sr));
        }

    }
    
    public void Heal(int heal) {
        if (heal > 0) {
            ChangeCurrentValue(heal);
            if (currentValue > MAX_VALUE) {
                SetCurrentValue(MAX_VALUE);
            }
        }

    }

    IEnumerator HurtColorShift(SpriteRenderer sr) {
        sr.color = hurtColor;
        yield return new WaitForSeconds(colorChangeSpeed);
        sr.color = normalColor;
        yield return null;
    }

    public void die() {
        dieEvent?.Invoke();
        hitBox.triggerEnter -= GetHit;
    }

    private void GetHit(Collider2D collision) {
        //if the thing damaging is an attack
        if (collision.GetComponent<AttackInfo>()) {
            //checks to see if the attack would be blocked
            AttackInfo attackInfo = collision.GetComponent<AttackInfo>();
            if (blockState?.IsBlockingAttack(attackInfo) ?? false) {
                hitEvent?.Invoke(false, null);
            } else {
                Damage(attackInfo.damage);
                hitEvent?.Invoke(true, attackInfo);
                //damageAmount = attackInfo.damage;
                attackInfo.VisualEffect();
                
            }
            //checks to see if the thing colliding can damage you at all
        } else if (collision.GetComponent<DamageScript>()) {
            DamageScript damageScript = collision.GetComponent<DamageScript>();
            Damage(damageScript.damage);
            hitEvent?.Invoke(true, damageScript);
        }
    }
    [ContextMenu("Heal")]
    private void Heal10() {
        Heal(10);
    }

}
