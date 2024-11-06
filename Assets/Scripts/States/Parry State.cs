using UnityEngine;

public class ParryState : State
{
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] private float timeFreezeTime;
    [SerializeField] private float shakeTime;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float timeSpeed;
    private float counter;
    public override void Enter() {
        counter = 0;
        interuptable = false;
        animator.Play(parryClip.name);
        Invoke("Exit", parryClip.length);
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
        }
    }*/
}
