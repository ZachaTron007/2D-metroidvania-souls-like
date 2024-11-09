using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dashScript : State
{
    
    //protected new bool interuptable = false;
    private IncludeRBLayers layers;
    [SerializeField] private AnimationClip dashClip;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashduration = 0.2f;
    public bool dashing;

    public IEnumerator dash() {
        rb.linearVelocity = Vector2.right * unitVariables.GetDirection() * dashSpeed;
        rb.excludeLayers = unitVariables.includeRBLayers.LayerMaskCreator(new int[] {3, 7, 8});
        yield return new WaitForSeconds(dashduration);
        rb.excludeLayers = unitVariables.includeRBLayers.LayerMaskCreator(new int[]{3, 7});
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
