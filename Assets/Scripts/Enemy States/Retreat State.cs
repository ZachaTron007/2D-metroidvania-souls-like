using NUnit.Framework;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using UnityEngine;

public class RetreatState : State
{
    private int retreatDirection;
    private float startPos;
    private void Awake() {
        interuptable = .1f;
    }
    protected override void ArgReciver(float[] args) {

        retreatDirection = (int)args[0];
        startPos = args[1];

    }
    // Update is called once per frame
    public override void UpdateState() {
        base.UpdateState();
        
    }

    public override void FixedUpdateState() {
        base.FixedUpdateState();    
    }
}
