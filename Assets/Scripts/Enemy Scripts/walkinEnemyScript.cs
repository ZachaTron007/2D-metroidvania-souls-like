using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class walkinEnemyScript : EnemyScript {

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate() {
        wallCheck();
        rb.velocity = Vector2.right * direction * speed;
    }
}
