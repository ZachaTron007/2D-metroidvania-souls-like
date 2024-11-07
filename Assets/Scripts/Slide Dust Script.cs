using Sirenix.Utilities;
using UnityEngine;

public class SlideDustScript : MonoBehaviour
{
    private int direction;
    private SpriteRenderer sr;
    [SerializeField] private AnimationClip dustClip;
    private Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
        animator.Play(dustClip.name);
        sr = GetComponent<SpriteRenderer>();
        sr.flipY = direction > 0;
    }
}
