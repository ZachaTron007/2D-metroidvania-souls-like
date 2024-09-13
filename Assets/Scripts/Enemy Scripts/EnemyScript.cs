using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    protected float maxDist = .5f;
    protected Rigidbody2D rb;

    protected enum State {
        Idel,
        Attack
    }

    protected int WallCheck(int dir) {
        //layers to hit
        int layerNumber = 6;
        //trasnforms that number into a layer mask
        int layerMask = 1 << layerNumber;
        Vector3 startPosition = gameObject.transform.position + new Vector3(0f, .5f, 0f);
        Vector2 rayDirection = new Vector2(dir, 0f);
        RaycastHit2D hit = Physics2D.Raycast(startPosition, rayDirection, maxDist, layerMask);
        if (hit) {
            Debug.Log("Hitting a " + hit.collider.gameObject.name);
            dir *= -1;
        }

        Debug.DrawRay(startPosition, rayDirection, Color.green, .5f);
        return dir;
    }

    /*private void FixedUpdate() {
        rb.velocity = Vector2.right * direction * speed;
    }*/
}
