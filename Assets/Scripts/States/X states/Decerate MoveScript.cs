using UnityEngine;

public class DecelerateMoveScript : State
{
    [SerializeField] private float decerationSpeed = 1;
    public float exitSpeed = 1;
    public override void FixedUpdateState() {
        base.FixedUpdateState();
        float speedDiffrence = 0 - rb.linearVelocityX;
        float movement = speedDiffrence * decerationSpeed;
        rb.AddForce(movement * Vector2.right);
        if (Mathf.Abs(rb.linearVelocityX) <= exitSpeed) {
            Exit();
        }
    }


}
