using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    [SerializeField] private int direction = 1;
    protected float maxDist = .5f;
    [SerializeField] protected float speed = 1;
    protected Rigidbody2D rb;

    

    protected int wallCheck(int dir) {
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * dir, maxDist, layerMask);
        if (hit) {
            Debug.Log("Hitting a wall");
            dir *= -1;
        }
        return dir;
    }

    /*private void FixedUpdate() {
        rb.velocity = Vector2.right * direction * speed;
    }*/
}
