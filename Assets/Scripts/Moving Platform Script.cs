using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private Vector2 endPoint;
    private Rigidbody2D rb;
    private Vector2 endPoint2;
    private Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endPoint2 = transform.position;
        direction = endPoint - endPoint2;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = (Vector3)direction*moveSpeed;
    }
}
