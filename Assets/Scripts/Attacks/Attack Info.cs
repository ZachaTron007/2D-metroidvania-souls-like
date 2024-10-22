using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo : DamageScript
{
    public AnimationClip clip;
    public float length;
    public BoxCollider2D attackHitBox;
    [SerializeField] private float ScreenShakeMagnitude;


    private void Awake() {
        length = clip.length;
        attackHitBox = GetComponent<BoxCollider2D>();
    }

    public virtual void VisualEffect() {
        CinemachineEffectScript.instance.ScreenShake(ScreenShakeMagnitude, .2f);
    }
}
