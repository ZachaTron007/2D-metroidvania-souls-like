using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
public class charginEnemy : EnemyScript
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float maxPlayerDist = 1;
    [SerializeField] private float chargeSpeed = 10;
    [SerializeField] private float retreatSpeed = -1;
    [SerializeField] private float retreatTimer = 1;
    [SerializeField] private float retreatTime;
    [SerializeField] private float dazeTimer = 1;
    [SerializeField] private float dazeTime;
    [SerializeField] private float collsiionDist = .2f;
    [SerializeField] private float bounceDist = .2f;
    private int direction;

    [SerializeField] private bool idel = true;
    [SerializeField] private bool move = true;
    [SerializeField] private bool hit=false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        idel = true;
        
    }

    void Update() {
        direction=wallCheck(direction);

        if (idel) {
            move = true;
            //resets the speed after charaging
            speed = moveSpeed;
            retreatTime = 0;
            //detecting the player
            RaycastHit2D playerHit = Physics2D.Raycast(transform.position, Vector2.right * direction, maxPlayerDist, 8);
            if (playerHit) {
                idel = false;
            }
        }else {
            speed = retreatSpeed;
            retreatTime += Time.deltaTime;

            if (retreatTime>=retreatTimer) {
                speed = chargeSpeed;
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * direction, collsiionDist, 64);
                RaycastHit2D playerHit = Physics2D.Raycast(transform.position, Vector2.right * direction, collsiionDist, 8);

                if (wallHit || playerHit) {
                    move = false;
                    hit = true;
                    //Debug.Log(dazeTime);
                }
                if (hit) {
                    //Debug.Log(dazeTime);
                    if (dazeTime == 0) {
                        //Debug.Log("dazed. Can move: "+move);
                        rb.AddForce(Vector2.right * -direction * bounceDist);
                    }
                    speed = 0;
                    if (dazeTime >= dazeTimer) {
                        idel = true;
                        hit = false;
                        dazeTime = 0;

                    }else
                        dazeTime += Time.deltaTime;
                }
            }
        }
    }

    private void FixedUpdate() {
        if (move) {
            rb.velocity = Vector2.right * direction * speed;
        }
    }
}
*/