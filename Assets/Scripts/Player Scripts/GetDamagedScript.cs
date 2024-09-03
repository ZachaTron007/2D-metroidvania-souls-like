using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamagedScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<DamageScript>()!=null) {
            Health1 health = GetComponent<Health1>();
            DamageScript damageScript = collision.GetComponent<DamageScript>();
            health.Damage(damageScript.damage);

        }
    }
}
