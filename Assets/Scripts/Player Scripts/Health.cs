using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health1 : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int MAX_HEALTH = 100;
    private Vector4 hurtColor = new Vector4(255, 62, 62, 255);
    private float colorChangeSpeed = 0.7f;

    // Update is called once per frame
    private void Awake() {
        health = MAX_HEALTH;
    }

    void Update()
    {

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
            //StartCoroutine(Hurt());
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
        sr.color = hurtColor / 255;
        yield return new WaitForSeconds(colorChangeSpeed);
        sr.color = Color.white;
        yield return null;
    }

    public void die() {
        Debug.Log("You died");
        Destroy(gameObject);
    }
}
