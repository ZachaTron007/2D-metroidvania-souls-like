using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class Healths1 : MonoBehaviour {
    private PlayerState player;

    private int health = 100;
    private int MAX_HEALTH = 100;
    private Vector4 hurtColor = new Vector4(255, 62, 62, 255);
    private Vector4 normalColor;
    private float colorChangeSpeed = 0.7f;
    private SpriteRenderer sr;
    private Sensors hitBox;
    //[SerializeField] private float parryWindow = 5f;
    private event Action getHitEvent;
    public event Action dieEvent;
    public bool blocking = false;

    // Update is called once per frame
    private void Awake() {
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
            StartCoroutine(Hurt(sr));
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

    IEnumerator Hurt(SpriteRenderer sr) {
        sr.color = hurtColor;
        yield return new WaitForSeconds(colorChangeSpeed);
        sr.color = normalColor;
        yield return null;
    }

    public void die() {
        dieEvent?.Invoke();
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!blocking) {
            if (collision.GetComponent<DamageScript>() != null) {
                DamageScript damageScript = collision.GetComponent<DamageScript>();
                Damage(damageScript.damage);
            }
        }
    }

    private void GetHit(Collider2D collision) {
        Debug.Log("Get Hit");
    }

}
