using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class MoveState : RbVelocitySlerp
{
    [SerializeField] private float moveSpeed = 5;
    private float biDirectionalWeight = 1;
    private float weight;
    private float startWeight = 1;
    private float endWeight = 0;
    
    public float forceAdded = 2;
    public float totalSpeedTransfer = .5f;
    private float weightTransferSpeed = .5f;
    private float counter;
    public override void Enter() {
        base.Enter();
        animator.Play(unitVariables.animations.runAnimation.name);
    }
    public override void FixedUpdateState() { 
        
        base.FixedUpdateState();
        rb.AddForce(movement*Vector2.right);
        //Debug.Log("Adding Force");
    }
    

    public void Update() {
        if (counter > 0) {
            counter -= Time.deltaTime;
            biDirectionalWeight = Mathf.Lerp(startWeight, endWeight, 1 - (counter / weightTransferSpeed));
        }
    }

    public void SetBiDirectionalWeight(float startWeight, float goalWeight, float speed = .5f) {
        endWeight = goalWeight;
        this.startWeight = startWeight;
        weightTransferSpeed = speed;
        counter = weightTransferSpeed;
    }
    public void SetWeight(float newWeight) {
        weight = newWeight;
    }
    protected override float GetTargetSpeed() {
        dir = Vector2.right;
        return (unitVariables.GetDirection() == Mathf.Sign(biDirectionalWeight)) ? moveSpeed * unitVariables.GetDirection() * Mathf.Abs(biDirectionalWeight) + weight : moveSpeed * unitVariables.GetDirection() + weight;
    }
}
