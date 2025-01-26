using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic; 
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class dashScript : RbVelocityLerp {
    
    //protected new bool interuptable = false;
    private IncludeRBLayers layers;
    [HideInInspector] public int dashCount = 1;
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashSpeedLow = 3;
    [SerializeField] private float dashduration = 0.2f;
    //[SerializeField] private float accelRate = 1;
    //[SerializeField] private float decelRate = 1;
    [SerializeField] private float forceExitTime = .2f;
    [SerializeField] private float speed;
    
    private float effectSpeed = 1.7f;
    private float xOffset = .8f;
    private float yOffset = .5f;
    private float destroyDelay = .4f;
    [Header("Effect Settings")]
    [SerializeField] private GameObject eeffect;
    private BoxCollider2D hitBox;
    public bool dashing;

    private void Start() {
        hitBox = unitVariables.mainCollider.GetComponent<BoxCollider2D>();
    }
    
    public override void Enter() {
        base.Enter();
        speed = dashSpeed;
        ResetLerp(startSpeed: rb.linearVelocityX, totalTime: dashduration, Vector2.right);
        interuptable = .9f;
        animator.Play(unitVariables.animations.idelAnimation.name);
        hitBox.excludeLayers = HelperFunctions.LayerMaskCreator(new int[] { 3, 7, 8 });
        Vector3 startPos = unitVariables.transform.position;
        unitVariables.SpawnEffect(eeffect, startPos, Quaternion.Euler(0, 0, -90 * unitVariables.GetDirection()), destroyDelay,new Vector2(xOffset,yOffset), effectSpeed);
        rb.gravityScale = 0f;

    }
    protected override void FinishedLerping() {
        if (speed == dashSpeed){
            speed = dashSpeedLow;
            ResetLerp(startSpeed: dashSpeed, totalTime: forceExitTime, Vector2.right);
        }else { Exit(); }
    }
    protected override float GetTargetSpeed() {
        return speed * unitVariables.GetDirection();
    }

    public override void Exit() {
        base.Exit();
        rb.gravityScale = HelperFunctions.regularGavityScale;
        dashCount -= 1;
    }
}
