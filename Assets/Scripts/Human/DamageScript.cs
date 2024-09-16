using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public int damage = 10 ;
    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void OnDisable()
    {
        
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Health1>() != null) {
            Health1 health = collision.GetComponent<Health1>();
            health.Damage(damage);
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.GetComponent<Health1>() != null) {
            Health1 health = collision.collider.GetComponent<Health1>();
            health.Damage(damage);

        }
    }*/
}
