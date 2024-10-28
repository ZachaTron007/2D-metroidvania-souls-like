using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo : DamageScript
{
    public AnimationClip clip;
    public float length;
    public BoxCollider2D attackHitBox;
    [SerializeField] private float ScreenShakeMagnitude;
    [Header("Frame data")]
    [SerializeField] private int totalFrames;
    public float startHitBoxTime;
    public float endHitBoxTime;


    private void Awake() {
        length = clip.length;
        attackHitBox = GetComponent<BoxCollider2D>();
        float timePerFrame = clip.length / totalFrames;
        startHitBoxTime = timePerFrame * startHitBoxTime;
        endHitBoxTime = timePerFrame * Mathf.Abs(endHitBoxTime - startHitBoxTime);
        //startHitBoxTime = startHitBoxTime/ clip.frameRate;

    }
    private void Update() {
        Debug.Log(startHitBoxTime + endHitBoxTime);
    }

    public virtual void VisualEffect() {
        CinemachineEffectScript.instance.ScreenShake(ScreenShakeMagnitude, .2f);
    }
}
