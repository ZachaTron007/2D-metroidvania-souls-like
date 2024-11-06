using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;


public class Health : MonoBehaviour
{

    [SerializeField] private BlockState blockState;
    public int health = 100;
    public int MAX_HEALTH = 100;
    private Vector4 hurtColor = new Vector4(255, 62, 62, 255);
    private Vector4 normalColor;
    private float colorChangeSpeed = 0.2f;
    private SpriteRenderer sr;
    [SerializeField] private Sensors hitBox;
    //[SerializeField] private float parryWindow = 5f;
    public event Action<bool, int> hitEvent;
    public event Action dieEvent;
    private PlayerState playerState;
    private int damageAmount = 0;

    // Update is called once per frame
    
    private void Awake() {
        playerState = GetComponent<PlayerState>();
        health = MAX_HEALTH;
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
        hurtColor /= 255;
        hitBox.triggerEnter += GetHit;
    }

    void Update() {
        if (health <= 0) {
            die();
        }
    }


    public void Damage(int damage) {
        if (damage > 0) {
            health -= damage;
            if (health < 0) {
                health = 0;
            }
            StartCoroutine(HurtColorShift(sr));
        }

    }
    public void Heal(int heal) {
        if (heal > 0) {
            health += heal;
            if (health > MAX_HEALTH) {
                health = MAX_HEALTH;
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
        //Destroy(gameObject);
    }

    private void GetHit(Collider2D collision) {
        //if the thing damaging is an attack
        if (collision.GetComponent<AttackInfo>()) {
            //checks to see if the attack would be blocked
            AttackInfo attackInfo = collision.GetComponent<AttackInfo>();
            if (blockState?.IsBlockingAttack(attackInfo) ?? false) {
                hitEvent?.Invoke(false, 0);
            } else {
                Damage(attackInfo.damage);
                hitEvent?.Invoke(true, -attackInfo.damage);
                //damageAmount = attackInfo.damage;
                attackInfo.VisualEffect();
                
            }
            //checks to see if the thing colliding can damage you at all
        } else if (collision.GetComponent<DamageScript>()) {
            DamageScript damageScript = collision.GetComponent<DamageScript>();
            Damage(damageScript.damage);

        }
    }


}
