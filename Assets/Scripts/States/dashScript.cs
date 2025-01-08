using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dashScript : State
{
    
    //protected new bool interuptable = false;
    private IncludeRBLayers layers;
    public int dashCount = 1;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashduration = 0.2f;
    [SerializeField] private float effectSpeed = 1;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float destroyDelay = .5f;
    [SerializeField] private GameObject eeffect;
    private BoxCollider2D hitBox;
    public bool dashing;

    private void Start() {
        hitBox = unitVariables.mainCollider.GetComponent<BoxCollider2D>();
    }

    public IEnumerator dash() {
        rb.linearVelocity = Vector2.right * unitVariables.GetDirection() * dashSpeed;
        hitBox.excludeLayers = HelperFunctions.LayerMaskCreator(new int[] {3, 7, 8});
        Vector3 startPos = unitVariables.transform.position;
        GameObject effect = Instantiate(eeffect, new Vector3(startPos.x + (xOffset * unitVariables.GetDirection()), startPos.y + yOffset, startPos.z), Quaternion.Euler(0, 0, -90*unitVariables.GetDirection()));
        Destroy(effect, destroyDelay);
        effect.GetComponent<Animator>().speed = effectSpeed;
        yield return new WaitForSeconds(dashduration);
        hitBox.excludeLayers = HelperFunctions.LayerMaskCreator(new int[]{3, 7});
        Exit();
        yield return null;
    }
    
    public override void Enter() {
        
        interuptable = 1f;
        animator.Play(unitVariables.animations.idelAnimation.name);
        StartCoroutine(dash());
    }
    public override void Exit() {
        dashCount -= 1;
        rb.linearVelocity = Vector2.zero;
        stateDone = true;
    }
}
