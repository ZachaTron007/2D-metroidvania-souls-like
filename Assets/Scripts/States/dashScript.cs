using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dashScript : State
{
    
    //protected new bool interuptable = false;
    [SerializeField] private AnimationClip dashClip;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashduration = 0.2f;
    public bool dashing;

    public IEnumerator dash() {
        LayerMask layer1 = 1 << 7;
        LayerMask layer2 = 1 << 8;
        LayerMask layer3 = 1 << 3;
        LayerMask layermask = ~(layer1| layer2 | layer3);
        rb.linearVelocity = Vector2.right * unitVariables.direction * dashSpeed;
        rb.excludeLayers = layermask;
        yield return new WaitForSeconds(dashduration);

        Exit();
        yield return null;
    }

    
    public override void UpdateState() {

    }
    public override void Enter() {
        interuptable = false;
        if (dashClip) {
            animator.Play(dashClip.name);
        }
        StartCoroutine(dash());
    }
    public override void Exit() {
        
        rb.linearVelocity = Vector2.zero;
        stateDone = true;
    }
    private void dashReset() {
        dashing = false;
    }
}
