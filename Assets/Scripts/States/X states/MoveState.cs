using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class MoveState : LerpingInhertedClass {
    [SerializeField] private float moveSpeed = 5;
    private float biDirectionalWeight = 1;
    private float weight;
    private float startWeight = 1;
    private float endWeight = 0;
    public float forceAdded = 2;
    public float totalSpeedTransfer = .5f;
    private float weightTransferSpeed = .5f;
    private float counterr;
    protected override void Start() {
        base.Start();
        clip = unitVariables.animations.runAnimation;
    }
    public override void Enter() {
        base.Enter();
        float unWeightedSpeed = moveSpeed * unitVariables.GetDirection() + weight;
        float weightedSpeed = moveSpeed * unitVariables.GetDirection() * Mathf.Abs(biDirectionalWeight) + weight;
        float TargetSpeed = (unitVariables.GetDirection() == Mathf.Sign(biDirectionalWeight)) ? weightedSpeed : unWeightedSpeed;
        movment[0] = new RbVelocityLerp(startSpeed: (rb.linearVelocityX > moveSpeed) ? rb.linearVelocityX : 0, TargetSpeed, totalTime, Vector2.right,rb,curve);
    }
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float unWeightedSpeed = moveSpeed * unitVariables.GetDirection() + weight;
        float weightedSpeed = moveSpeed * unitVariables.GetDirection() * Mathf.Abs(biDirectionalWeight) + weight;
        float TargetSpeed = (unitVariables.GetDirection() == Mathf.Sign(biDirectionalWeight)) ? weightedSpeed : unWeightedSpeed;
        movment[0].ChangeTargetSpeed(TargetSpeed);
    }
    

    public void Update() {
        if (counterr > 0) {
            counterr -= Time.deltaTime;
            biDirectionalWeight = Mathf.Lerp(startWeight, endWeight, 1 - (counterr / weightTransferSpeed));
        }
    }

    public void SetBiDirectionalWeight(float startWeight, float goalWeight, float speed = .5f) {
        endWeight = goalWeight;
        this.startWeight = startWeight;
        weightTransferSpeed = speed;
        counterr = weightTransferSpeed;
    }
    public void SetWeight(float newWeight) {
        weight = newWeight;
    }

    protected override void StateIsDone() {
        base.StateIsDone();
    }
}
