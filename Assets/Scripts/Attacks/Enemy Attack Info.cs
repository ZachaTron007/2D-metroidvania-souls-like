using UnityEngine;

public class EnemyAttackInfo : AttackInfo {

    //[SerializeField] private float ScreenShakeMagnitude;
    public override void VisualEffect() {
        base.VisualEffect();
        PostProcessingEffectScript.instance.HurtVignette();
    }
}
