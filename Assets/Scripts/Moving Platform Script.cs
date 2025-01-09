using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private Vector2 endPoint;
    private Rigidbody2D rb;
    private Vector2 endPoint2;
    private Vector2 direction;
    private float totalDistance;
    private bool canTurn = true;
    public float StayTime = 2;
    private float counter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        endPoint2 = transform.position;
        direction = endPoint - endPoint2;
        direction.Normalize();
        totalDistance = Mathf.Abs(HelperFunctions.PointToDistance(endPoint2, endPoint));
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = (Vector3)direction*moveSpeed*Time.deltaTime;
        float distance = HelperFunctions.PointToDistance(transform.position, endPoint);
        float distance2 = HelperFunctions.PointToDistance(transform.position, endPoint2);

        if (canTurn) {
            if (distance > totalDistance || distance2 > totalDistance) {
                
                
                counter += Time.deltaTime;
                if (counter < StayTime) {
                    rb.linearVelocity = Vector3.zero;
                } else {
                    direction *= -1;
                    canTurn = false;
                    rb.linearVelocity = (Vector3)direction * moveSpeed * Time.deltaTime;
                }
                Invoke(nameof(ResetCounter), StayTime+1);
            }
        }
    }

    private void ResetCounter() {
        canTurn = true;
        counter = 0;
    }
}
