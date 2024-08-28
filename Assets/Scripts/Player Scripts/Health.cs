using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health1 : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int MAX_HEALTH = 100;

    // Update is called once per frame
    private void Awake() {
        health = MAX_HEALTH;
    }

    void Update()
    {

        if (health <= 0) {
            Debug.Log("You died");
            Destroy(gameObject);
        }
    }


    public void Damage(int damage) {
        if (damage > 0) {
            health -= damage;
            if (health < 0) {
                health = 0;
            }
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
}
