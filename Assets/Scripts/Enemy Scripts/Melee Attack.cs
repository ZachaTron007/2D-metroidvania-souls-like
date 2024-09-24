using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private BoxCollider2D attackHitBox;
    [SerializeField] private float attackTime = .25f;
    [SerializeField] private float knockback = 5f;
    
    private Vector2 offset;
    public Vector2 lookDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        offset.x = 0.87f;
        offset.y = 1.27f;
        attackHitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }/*
    public void Attack() {
        Invoke("AttackStart", attackTime);
        
    }

    private void AttackEnd() {
        attackHitBox.enabled = false;
    }
    private void AttackStart() {
        Invoke("AttackEnd", 1);
        attackHitBox.enabled = true;
        attackHitBox.offset = offsetVector();
    }*/

    private Vector2 offsetVector(float direction) {
        Vector2 dir = new Vector2(direction*offset.x,offset.y);
        return dir;
    }

    public IEnumerator Attack(float direction) {
        float startAttack = .15f;
        yield return new WaitForSeconds(startAttack);
        attackHitBox.enabled = true;
        attackHitBox.offset = offsetVector(direction);
        float endAttack = .15f;
        yield return new WaitForSeconds(endAttack);
        attackHitBox.enabled = false;
    }
    
}
