using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class archerEnemyScript: MonoBehaviour {
    [SerializeField] private int direction = 1;
    [SerializeField] private float maxDist = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private float fireCount;

    [SerializeField] private bool idel;
    private int stopped = 1;
    Rigidbody2D rb;

    [SerializeField] GameObject bullet;

    void Start() {

        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (idel) {
            stopped = 1;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, maxDist, 72);
            if (hit)
                direction *= -1;
        } else {
            if (fireCount >= fireRate) {
                Instantiate(bullet, transform.position, transform.rotation, transform);
                //Instantiate(bullet, transform,false);
                fireCount = 0;
            } else
                fireCount += Time.deltaTime;
            stopped = 0;
        }
    }

    private void FixedUpdate() {
        rb.linearVelocity = Vector2.right * direction * speed * stopped;
    }
}
