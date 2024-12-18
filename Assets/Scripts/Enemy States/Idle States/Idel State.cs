using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IdelState : BaseIdelState {
    // Start is called before the first frame update
    //movements
    private float switchCounter;
    private float switchTime = 5;
    [SerializeField] private float lowSwitchTime = 1;
    [SerializeField] private float highSwitchTime = 2;
    private IdelFunctionalityDelegate[] IdelFunctionalities;

    private void Awake() {
        IdelFunctionalities = new IdelFunctionalityDelegate[] { Walk, Stay };
    }

    // Update is called once per frame
    public override void Enter() {
        base.Enter();
        switchTime = Random.Range(lowSwitchTime, highSwitchTime);
        switchCounter = 0;
        IdelFunctionality = Stay;
        
    }
    protected override void Stay() {
        base.Stay();
    }
    private void Walk() {
        animator.Play(walkAnimation.name);
        rb.linearVelocity = new Vector2(unitVariables.GetDirection() * walkSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
    
    public override void FixedUpdateState() {
        //rb.linearVelocity = new Vector2(unitVariables.GetDirection() * speed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }

    public override void UpdateState() {
        base.UpdateState();
        switchCounter += Time.deltaTime;
        if (switchCounter > switchTime) {
            IdelFunctionality = IdelFunctionalities[Random.Range(0, IdelFunctionalities.Length)];
            switchTime = Random.Range(lowSwitchTime, highSwitchTime);
            switchCounter = 0;
        }
        IdelFunctionality();
    }
    
}
