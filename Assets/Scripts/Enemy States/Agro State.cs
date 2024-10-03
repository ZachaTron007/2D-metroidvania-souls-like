using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroState : State
{
    public override void Enter() {
        int agroSpeed = 150;
        playerVariables.moveSpeed = agroSpeed;
    }
    public override void FixedUpdateState() {
        rb.velocity = new Vector2(playerVariables.direction * playerVariables.moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }
}
