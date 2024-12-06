using UnityEngine;

public class StunnedState : State
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AnimationClip stunClip;
    void Awake()
    {
        interuptable = .8f;
    }

    public void Enter() {
        animator.Play(stunClip.name);
    }
}
