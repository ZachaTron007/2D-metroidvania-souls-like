using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : State
{
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    [SerializeField] private float lowJumpMultiplier = 20f;
    public bool grounded = false;
    public float kyoteTime;
    public int doubleJump = 0;
    [SerializeField] private float yoffset = .1f;
    [SerializeField] private float destroyDelay = .5f;
    [SerializeField] private float effectSpeed = 0;
    [SerializeField] private GameObject puff;

    // Start is called before the first frame update
    public float grav = 2;

    // Update is called once per frame

    public void Jump() {
        //wallSliding = false;
        //makes the y component change
        rb.linearVelocity = Vector2.up * jumpVelocity;
        
        rb.gravityScale = grav;
        doubleJump += 1;

    }

    public override void UpdateState() {
        if (rb.linearVelocity.y <= 0) {
            Exit();
        }

    }
    public override void FixedUpdateState() {
        if (!Input.GetKey(KeyCode.Space)) {
            rb.linearVelocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - rb.gravityScale) * Time.deltaTime;
        }
        
    }

    public override void Enter() {
        GameObject effect = Instantiate(puff, new Vector3(unitVariables.transform.position.x, unitVariables.transform.position.y + yoffset, unitVariables.transform.position.z),new Quaternion(0,0,0,0));
        Destroy(effect,destroyDelay);
        effect.GetComponent<Animator>().speed = effectSpeed;
        //jumpVelocity = Mathf.Sqrt(Physics.gravity.y * 2 * jumpHeight * -2);
        animator.Play(unitVariables.animations.jumpAnimation.name);
        Jump();
    }
    public override void Exit() {
        stateDone = true;
        //rb.velocity = Vector2.zero;
    }

}
