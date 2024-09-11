using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    [SerializeField] private int direction = 1;
    protected float maxDist = .5f;
    [SerializeField] protected float speed = 1;
    protected Rigidbody2D rb;
    [SerializeField] protected Vector2 position;

    

    protected int wallCheck(int dir) {
        int layerNumber = 6;
        int layerMask = 1 << layerNumber;
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, position, maxDist, layerMask);
        if (hit) {
            Debug.Log("Hitting a "+hit.collider.gameObject.name);
            dir *= -1;
        }
        Debug.DrawRay(transform.position, position,Color.green, maxDist);
        return dir;
    }

    /*private void FixedUpdate() {
        rb.velocity = Vector2.right * direction * speed;
    }*/
}
