using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : DamageScript
{
    public AnimationClip clip;
    public float length;

    private void Awake() {
        length = clip.length;
    }
}
