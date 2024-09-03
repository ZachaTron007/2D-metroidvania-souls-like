using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    [SerializeField] protected int direction = 1;
    [SerializeField] protected float maxDist = 1;
    [SerializeField] protected float speed = 1;
    protected Rigidbody2D rb;

    

    protected void wallCheck() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, maxDist, 72);
        if (hit)
            direction *= -1;
    }

    private void FixedUpdate() {
        rb.velocity = Vector2.right * direction * speed;
    }
}
