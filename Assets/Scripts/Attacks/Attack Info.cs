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
    public float speed = 1;
    [Header("Frame data")]
    [SerializeField] private float startHitBoxFrames;
    [SerializeField] private float endHitBoxFrames;
    [SerializeField] private float startMovingFrames;
    [HideInInspector] public float startMovingTime;
    [HideInInspector] public float startHitBoxTime;
    [HideInInspector] public float endHitBoxTime;



    private void Awake() {
        float inverseSpeed = 1 / speed;
        length = clip.length * inverseSpeed;
        attackHitBox = GetComponent<BoxCollider2D>();
        float timePerFrame = (1 / clip.frameRate) * inverseSpeed;
        startMovingTime = timePerFrame * startMovingFrames;
        startHitBoxTime = timePerFrame * startHitBoxFrames - startMovingTime;
        endHitBoxTime = timePerFrame * endHitBoxFrames - startHitBoxTime;
        /*
        startHitBoxFrames *= speed;
        endHitBoxFrames *= speed;
        startMovingFrames *= speed;
        clip.spee = speed;*/
        

    }
    private void Update() {

    }

    public virtual void VisualEffect() {
        CinemachineEffectScript.instance.ScreenShake(ScreenShakeMagnitude, .2f);
    }
}
