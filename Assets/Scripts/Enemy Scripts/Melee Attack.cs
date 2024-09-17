using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private BoxCollider2D attackHitBox;
    private float attackTime = .25f;
    [SerializeField] private float knockback = 5f;
    
    [SerializeField] private Vector2 offset;
    public Vector2 lookDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        offset.x = 0.87f;
        offset.y =1.27f;
        attackHitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack() {
        Invoke("AttackEnd", attackTime);
        
        attackHitBox.enabled = true;
        attackHitBox.offset = offsetVector();
    }

    private void AttackEnd() {
        attackHitBox.enabled = false;
    }

    private Vector2 offsetVector() {
        Vector2 dir = lookDirection*offset;
        return dir;
    }
    
}
