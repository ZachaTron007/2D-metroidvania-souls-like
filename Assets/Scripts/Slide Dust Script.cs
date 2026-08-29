using Sirenix.Utilities;
using UnityEngine;

public class SlideDustScript : MonoBehaviour
{
    public int direction;
    private SpriteRenderer sr;
    [SerializeField] private AnimationClip dustClip;
    private Animator animator;
    void Awake() {
        animator = GetComponent<Animator>();
        animator.Play(dustClip.name);
        sr = GetComponent<SpriteRenderer>();
        sr.flipY = direction < 0;
    }

    public void SetDirection(int newDirection) {
        direction = newDirection;
        sr.flipY = direction < 0;
    }
}
