using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic; 
using Unity.VisualScripting;
using UnityEngine;

public class dashScript : RbVelocityLerp {
    
    //protected new bool interuptable = false;
    private IncludeRBLayers layers;
    public int dashCount = 1;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashSpeedLow = 3;
    [SerializeField] private float dashduration = 0.2f;
    [SerializeField] private float accelRate = 1;
    [SerializeField] private float decelRate = 1;
    [SerializeField] private float forceExitTime = .2f;
    [SerializeField] private float speed;
    private float effectSpeed = 1.7f;
    private float xOffset = .8f;
    private float yOffset = .5f;
    private float destroyDelay = .4f;
    [SerializeField] private GameObject eeffect;
    private BoxCollider2D hitBox;
    public bool dashing;

    private void Start() {
        hitBox = unitVariables.mainCollider.GetComponent<BoxCollider2D>();
    }

    public IEnumerator dash() {
        /*
        rb.linearVelocity = Vector2.right * unitVariables.GetDirection() * dashSpeed;
        hitBox.excludeLayers = HelperFunctions.LayerMaskCreator(new int[] {3, 7, 8});
        Vector3 startPos = unitVariables.transform.position;
        GameObject effect = Instantiate(eeffect, new Vector3(startPos.x + (xOffset * unitVariables.GetDirection()), startPos.y + yOffset, startPos.z), Quaternion.Euler(0, 0, -90*unitVariables.GetDirection()));
        Destroy(effect, destroyDelay);
        effect.GetComponent<Animator>().speed = effectSpeed;
        yield return new WaitForSeconds(dashduration);
        hitBox.excludeLayers = HelperFunctions.LayerMaskCreator(new int[]{3, 7});
        Exit();
        yield return null;*/
        return null;
    }
    
    public override void Enter() {
        base.Enter();
        speed = dashSpeed;
        accSpeed = accelRate;
        interuptable = .9f;
        animator.Play(unitVariables.animations.idelAnimation.name);
        hitBox.excludeLayers = HelperFunctions.LayerMaskCreator(new int[] { 3, 7, 8 });
        Vector3 startPos = unitVariables.transform.position;
        GameObject effect = Instantiate(eeffect, new Vector3(startPos.x + (xOffset * unitVariables.GetDirection()), startPos.y + yOffset, startPos.z), Quaternion.Euler(0, 0, -90 * unitVariables.GetDirection()));
        Destroy(effect, destroyDelay);
        effect.GetComponent<Animator>().speed = effectSpeed;
        Invoke(nameof(SlowDown),dashduration);
        rb.gravityScale = 2f;
        //StartCoroutine(dash());

    }
    private void SlowDown() {
        speed = dashSpeedLow;
        accSpeed = decelRate;
        interuptable = 0f;
        Invoke(nameof(Exit),forceExitTime);
    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        rb.AddForce(speed*unitVariables.GetDirection()*Vector2.right);

    }
    protected override float GetTargetSpeed() {
        return speed;
    }

    public override void Exit() {
        base.Exit();
        dashCount -= 1;
    }
}
