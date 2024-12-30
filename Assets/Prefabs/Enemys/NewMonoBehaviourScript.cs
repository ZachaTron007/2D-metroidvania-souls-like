using UnityEngine;
[CreateAssetMenu(fileName = "/scriptAble", menuName = "Animation Clips")]
public class AnimationCollection : ScriptableObject
{
    public AnimationClip runAnimation;
    public AnimationClip idelAnimation;
    public AnimationClip HurtAnimation;
    public AnimationClip parryAnimation;
    public AnimationClip deathAnimation;
    public AnimationClip fallAniamtion;
    public AnimationClip jumpAnimation;
    public AnimationClip blockAnimation;
}
