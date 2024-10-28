using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo : DamageScript
{
    public AnimationClip clip;
    public float length;
    [HideInInspector] public BoxCollider2D attackHitBox;
    [SerializeField] private float ScreenShakeMagnitude;
    [Header("Frame data")]
    [SerializeField] private float startHitBoxFrames;
    [SerializeField] private float endHitBoxFrames;
    [HideInInspector] public float startHitBoxTime;
    [HideInInspector] public float endHitBoxTime;


    private void Awake() {
        length = clip.length;
        attackHitBox = GetComponent<BoxCollider2D>();
        float timePerFrame = 1 / clip.frameRate;
        startHitBoxTime = timePerFrame * startHitBoxTime;
        endHitBoxTime = timePerFrame * Mathf.Abs(endHitBoxTime - startHitBoxTime);

    }
    private void Update() {

    }

    public virtual void VisualEffect() {
        CinemachineEffectScript.instance.ScreenShake(ScreenShakeMagnitude, .2f);
    }
}
