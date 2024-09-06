using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private BoxCollider2D attackHitBox;
    private float attackTime = .25f;
    [SerializeField] private float knockback = 5f;
    // Start is called before the first frame update
    void Start()
    {
        attackHitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack() {
        Invoke("AttackEnd", attackTime);
        attackHitBox.enabled = true;
    }

    private void AttackEnd() {
        attackHitBox.enabled = false;
    }
}
