using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    protected float maxDist = .3f;
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;

    protected enum State {
        Idel,
        Attack,
        Agro
    }

    protected int WallCheck(int direction) {
        //layers to hit
        int layerNumber = 6;
        RaycastHit2D hit = ShootRay(direction, layerNumber, maxDist);
        if (hit) {
            direction *= -1;
        }
        
        return direction;
    }

    protected bool WithinAgroRange(int direction) {
        float distance = 5;
        //layers to hit
        int layerNumber = 3;
        RaycastHit2D hit = ShootRay(direction, layerNumber, distance);

        return hit;
    }
    protected bool WithinAttackRange(int direction) {
        float distance = .5f;
        //layers to hit
        int layerNumber = 3;
        RaycastHit2D hit = ShootRay(direction, layerNumber, distance);

        return hit;
    }

    protected RaycastHit2D ShootRay(int direction,int layerNumber,float dist) {
        //trasnforms that number into a layer mask
        int layerMask = 1 << layerNumber;
        Vector3 startPosition = gameObject.transform.position + new Vector3(transform.localScale.x/2 * direction,gameObject.transform.localScale.y / 2,0);
        Vector2 rayDirection = new Vector2(direction, 0f);
        RaycastHit2D hit = Physics2D.Raycast(startPosition, rayDirection, dist, layerMask);
        Debug.DrawRay(startPosition,rayDirection,Color.green,.1f);
        return hit;
    }

}
