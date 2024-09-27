using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dashScript : State
{
    //protected new bool interuptable = false;
    [SerializeField] private AnimationClip dashClip;
    [SerializeField] private float dashSpeed = 15;
    private float dashduration = 0.2f;
    public bool dashing;

    public IEnumerator dash(float direction,Rigidbody2D rb) {
        Invoke("dashReset", dashduration);
        dashing = true;
        while (dashing) {
            rb.velocity = Vector2.right * direction * dashSpeed;
            yield return new WaitForFixedUpdate();
        }
        Exit();
        yield return null;
    }

    
    public override void UpdateState() {

    }
    public override void Enter() {
        if (dashClip) {
            animator.Play(dashClip.name);
        }
        StartCoroutine(dash(playerVariables.direction,rb));
        playerVariables.stateDone = false;
        Debug.Log("Dash Start");
    }
    public override void Exit() {
        rb.velocity = Vector2.zero;
        stateDone = true;
    }
    private void dashReset() {
        dashing = false;
    }
}
