using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : State
{
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    [SerializeField] private float lowJumpMultiplier = 20f;
    public bool grounded = false;
    [HideInInspector] public float kyoteTime = .2f;
    [HideInInspector] public int remainingJumps = 0;
    private int totalJumps;
    private float yoffset = .5f;
    private float destroyDelay = .4f;
    private float effectSpeed = 1.5f;
    [SerializeField] private GameObject puff;
    private float grav = 2;

    // Update is called once per frame

    public void Jump() {
        //wallSliding = false;
        //makes the y component change
        rb.linearVelocity = new Vector2(rb.linearVelocityX,jumpVelocity);
        
        rb.gravityScale = grav;
        remainingJumps -= 1;

    }

    public override void UpdateState() {
        base.UpdateState();
        Debug.Log(rb.linearVelocityY);
        if (rb.linearVelocityY <= 0) {
            Exit();
        }

    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        if (!Input.GetKey(KeyCode.Space)) {
            rb.linearVelocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - rb.gravityScale) * Time.fixedDeltaTime;
        }
        
    }

    public override void Enter() {
        base.Enter();
        interuptable = .1f;
        GameObject effect = Instantiate(puff, new Vector3(unitVariables.transform.position.x, unitVariables.transform.position.y + yoffset, unitVariables.transform.position.z),new Quaternion(0,0,0,0));
        Destroy(effect,destroyDelay);
        effect.GetComponent<Animator>().speed = effectSpeed;
        //jumpVelocity = Mathf.Sqrt(Physics.gravity.y * 2 * jumpHeight * -2);
        animator.Play(unitVariables.animations.jumpAnimation.name);
        Jump();
    }
    public void ResetJumpAmount() => remainingJumps = totalJumps;

}
