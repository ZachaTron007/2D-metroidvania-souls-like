using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : State
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    [SerializeField] private float lowJumpMultiplier = 20f;
    [HideInInspector] public float kyoteTime = .2f;
    public int remainingAirBorneJumps = 0;
    private int TotalAirBorneJumps = 0;
    private float yoffset = .5f;
    private float destroyDelay = .4f;
    private float effectSpeed = 1.5f;
    [SerializeField] private GameObject puff;

    // Update is called once per frame

    public void Jump() {
        //makes the y component change
        unitVariables.unModifiedSpeed += new Vector2(0,jumpVelocity);
        Debug.Log("Jump");
        rb.gravityScale = HelperFunctions.regularGavityScale;
        remainingAirBorneJumps -= 1;

    }

    public override void UpdateState() {
        base.UpdateState();
        if (rb.linearVelocityY <= 0) {
            Exit();
        }

    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        if (!Input.GetKey(KeyCode.Space)) {
            unitVariables.unModifiedSpeed += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - rb.gravityScale) * Time.fixedDeltaTime;
        }
        
    }

    public override void Enter() {
        base.Enter();
        interuptable = .1f;
        unitVariables.SpawnEffect(puff,unitVariables.transform.position, new Quaternion(0, 0, 0, 0), destroyDelay,new Vector2(0,yoffset),effectSpeed: effectSpeed);

        //jumpVelocity = Mathf.Sqrt(Physics.gravity.y * 2 * jumpHeight * -2);
        animator.Play(unitVariables.animations.jumpAnimation.name);
        Jump();
    }
    public void ResetJumpAmount() => remainingAirBorneJumps = TotalAirBorneJumps;
    public void ResetDoubleJump() => remainingAirBorneJumps = 1;

}
