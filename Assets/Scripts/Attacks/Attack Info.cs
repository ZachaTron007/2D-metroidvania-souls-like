using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo : DamageScript
{
    public AnimationClip clip;
    public float length;
    [HideInInspector] public BoxCollider2D attackHitBox;
    [Header("Attack data")]
    [SerializeField] public bool parryable;
    [SerializeField] public bool moveWhile = true;
    [SerializeField] private float ScreenShakeMagnitude;
    [Header("Frame data")]
    [SerializeField] private float startHitBoxFrames;
    [SerializeField] private float endHitBoxFrames;
    [SerializeField] private float startMovingFrames;
    [HideInInspector] public float startMovingTime;
    [HideInInspector] public float startHitBoxTime;
    [HideInInspector] public float endHitBoxTime;



    private void Awake() {
        length = clip.length;
        attackHitBox = GetComponent<BoxCollider2D>();
        float timePerFrame = 1 / clip.frameRate;
        startHitBoxTime = timePerFrame * startHitBoxFrames;
        startMovingTime = timePerFrame * startMovingFrames;
        endHitBoxTime = timePerFrame * endHitBoxFrames;
        startHitBoxTime -= startMovingTime;
        endHitBoxFrames -= startHitBoxTime;

    }
    private void Update() {

    }

    public virtual void VisualEffect() {
        CinemachineEffectScript.instance.ScreenShake(ScreenShakeMagnitude, .2f);
    }
}
