using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LerpingInhertedClass : State
{
    [Header("Lerp Settings")]
    [SerializeField] protected float totalTime;
    private RbVelocityLerp axis1;
    protected List<RbVelocityLerp> movment = new List<RbVelocityLerp>() { };
    [SerializeField] protected AnimationCurve curve;
    protected virtual void Start() {
        movment.Add(axis1);
    }
    public virtual void FinishedLerping() {

    }
    public override void FixedUpdateState() {
        foreach (var item in movment) {
            item?.FixedUpdate();
        }
    }
}
