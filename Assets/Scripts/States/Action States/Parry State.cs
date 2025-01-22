using UnityEngine;

public class ParryState : State
{
    [SerializeField] private float timeFreezeTime;
    [SerializeField] private float shakeTime;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float timeSpeed;
    [SerializeField] private float StunAmount = 10;
    private float counter;
    public override void Enter() {
        base.Enter();
        counter = 0;
        interuptable = .3f;
        animator.Play(unitVariables.animations.parryAnimation.name);
        Invoke("Exit", unitVariables.animations.parryAnimation.length);

        //gets the refrence tothe attack that hit you, then gets the stun component from the root of the attack in the hiarchy, then changes the stun
        Transform enemyTransform = unitVariables.lastAttackToHit.transform;
        Transform rootTransform = unitVariables.lastAttackToHit.transform.root;
        while (enemyTransform.gameObject.layer != HelperFunctions.layers["Enemys"] &&enemyTransform!=rootTransform) {
            enemyTransform = enemyTransform.parent;
            
        }
        enemyTransform.TryGetComponent(out Stun stun);
        stun?.ChangeCurrentValue(StunAmount);
        CinemachineEffectScript.instance.ScreenShake(shakeIntensity, shakeTime);
        
    }
    public override void UpdateState() {
        /*
        base.UpdateState()
        counter += Time.deltaTime;
        if (counter < timeFreezeTime) {
            Time.timeScale = timeSpeed;
        } else {
            Time.timeScale = 1;
        }*/
    }
}
