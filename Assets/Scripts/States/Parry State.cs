using UnityEngine;

public class ParryState : MonoBehaviour
{
    [SerializeField] private AnimationClip parryClip;
    public void Enter() {
        animator.Play(parryClip.name);
        
    }
}
