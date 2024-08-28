using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class walkinEnemyScript : MonoBehaviour {
    [SerializeField] private int direction = 1;
    [SerializeField] private float maxDist = 1;
    [SerializeField] private float speed = 1;
    Rigidbody2D rb;

    void Start() {

        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, maxDist, 72);
        if (hit)
            direction *= -1;
    }

    private void FixedUpdate() {
        rb.velocity = Vector2.right * direction * speed;
    }
}
