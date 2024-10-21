using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : DamageScript
{
    public AnimationClip clip;
    public float length;
    public BoxCollider2D attackHitBox;
    public float ScreenShakeMagnitude;

    private void Awake() {
        length = clip.length;
        attackHitBox = GetComponent<BoxCollider2D>();
    }
}
