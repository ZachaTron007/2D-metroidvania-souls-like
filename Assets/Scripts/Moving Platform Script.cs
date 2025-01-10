using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private Vector2 endPoint;
    private Rigidbody2D rb;
    private Vector2 endPoint2;
    protected Vector2 direction;
    protected Vector2 goalPos;

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
        goalPos = endPoint;
        ResetCounter();
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpeed();
        float distance = HelperFunctions.PointToDistance(transform.position, endPoint);
        float distance2 = HelperFunctions.PointToDistance(transform.position, endPoint2);
        //Debug.Log(distance + " " + distance2);
        //Debug.Log(totalDistance);
        if (canTurn) {
            if (distance > totalDistance || distance2 > totalDistance) {


                if (counter == 0) {
                    Invoke(nameof(ResetCounter), StayTime + 1);
                }else if (counter < StayTime) {
                    rb.linearVelocity = Vector3.zero;
                } else {
                    direction *= -1;
                    canTurn = false;
                    MoveSpeed();
                }
                counter += Time.deltaTime;
                    
                if(distance > totalDistance) {
                    goalPos = endPoint;
                } else {
                    goalPos = endPoint2;
                }

            }
        }
    }
    protected virtual void MoveSpeed() {
        rb.linearVelocity = (Vector3)direction * moveSpeed * Time.deltaTime;
    }
    protected virtual void ResetCounter() {
        canTurn = true;
        counter = 0;
    }
}
