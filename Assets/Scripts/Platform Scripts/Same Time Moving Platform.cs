using UnityEngine;

public class SameTimeMovingPlatform : MovingPlatformScript
{
    private float moveTime;
    [SerializeField] private float totalMoveTime = 1f;
    private Vector2 startPos;
    protected override void MoveSpeed() {
        moveTime -= Time.deltaTime;
            if (moveTime <= 0) {
            transform.position = goalPos;
            }
            transform.position = new Vector3(Mathf.Lerp(startPos.x, goalPos.x, 1 - (moveTime / totalMoveTime)),Mathf.Lerp(startPos.y, goalPos.y, 1 - (moveTime / totalMoveTime)), 0);
    }
    protected override void ResetCounter() {
        base.ResetCounter();
        moveTime = totalMoveTime;
        startPos = transform.position;
        Vector2 dir = new Vector2(Mathf.Sign(startPos.x - goalPos.x), Mathf.Sign(startPos.y - goalPos.y));
        goalPos += direction*dir;
        //Debug.Log(goalPos);



    }
}
