using UnityEngine;

public class ParryState : State
{
    [SerializeField] private AnimationClip parryClip;
    [SerializeField] private float timeFreezeTime;
    private float counter;
    public override void Enter() {
        counter = 0;
        interuptable = false;
        animator.Play(parryClip.name);
        Invoke("Exit", parryClip.length);
        
    }
    public override void Exit() {
        stateDone = true;
    }
    public override void UpdateState() {
        /*
        counter += Time.deltaTime;
        if (counter < timeFreezeTime) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }*/
    }
}
