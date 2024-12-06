using UnityEngine;

public class ParryState : State
{
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] private float timeFreezeTime;
    [SerializeField] private float shakeTime;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float timeSpeed;
    [SerializeField] private float StunAmount = 10;
    private float counter;
    public override void Enter() {
        counter = 0;
        interuptable = .3f;
        animator.Play(parryClip.name);
        Invoke("Exit", parryClip.length);
        //gets the refrence tothe attack that hit you, then gets the stun component from the root of the attack in the hiarchy, then changes the stun
        unitVariables.lastAttackToHit.transform.root.GetComponent<Stun>().ChangeCurrentValue(StunAmount);
        CinemachineEffectScript.instance.ScreenShake(shakeIntensity, shakeTime);
        
    }
    public override void Exit() {
        stateDone = true;
    }
    public override void UpdateState() {
        /*
        counter += Time.deltaTime;
        if (counter < timeFreezeTime) {
            Time.timeScale = timeSpeed;
        } else {
            Time.timeScale = 1;
        }*/
    }
}
