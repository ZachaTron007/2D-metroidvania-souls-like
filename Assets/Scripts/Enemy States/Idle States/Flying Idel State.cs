using UnityEngine;
using UnityEngine.Rendering;

public class FlyingIdelState : BaseIdelState
{
    [SerializeField] private Vector3 point;
    private BoxCollider2D box;
    [SerializeField] private Sensors hitBoxScript;
    [SerializeField] private float idelSpeed;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private Vector2 dir;
    private Vector2 boundsLowerLeft;
    private Vector2 boundsUpperRight;

    private float timer;
    bool arrived = true;
    private void Start() {
        boundsLowerLeft = new Vector2(hitBoxScript.hitBox.bounds.min.x, hitBoxScript.hitBox.bounds.min.y);
        boundsUpperRight = new Vector2(hitBoxScript.hitBox.bounds.max.x, hitBoxScript.hitBox.bounds.max.y);
    }
    public override void Enter()
    {
        animator.Play(idelAniamtion.name);
    }

    private Vector3 PointPicker() {
        do {
            point = new Vector3(Random.Range(boundsLowerLeft.x, boundsUpperRight.x), Random.Range(boundsLowerLeft.y, boundsUpperRight.y), unitVariables.transform.position.z);
            dir = (point - unitVariables.transform.position).normalized;

        }
        while (ShootCheckRay());
        
        arrived = false;
        return point;
    }

    public override void UpdateState() {
        if (!arrived) {
            unitVariables.SetDirection((dir.x > 0) ? 1 : -1);
            rb.linearVelocity = idelSpeed * Time.deltaTime * dir;
            if (Vector2.Distance(unitVariables.transform.position, point) < 0.2f) {
                arrived = true;
            }
        } else {
            rb.linearVelocity = Vector2.zero;
            timer +=Time.deltaTime;
            if (timer > waitTime) {
                PointPicker();
                timer = 0;
            }
            
        }
    }
    private bool ShootCheckRay() {
        Vector2 offset = unitVariables.mainCollider.hitBox.offset;
        Vector2 size = unitVariables.mainCollider.hitBox.size/2;
        //Debug.Log("other layermask: " + layerMask);
        Vector3 startPos = new Vector3(unitVariables.transform.position.x + offset.x, unitVariables.transform.position.y + offset.y, unitVariables.transform.position.z);
        RaycastHit2D topRight = Physics2D.Raycast(new Vector3(startPos.x * size.x, startPos.y * size.y, startPos.z), dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.LayerMaskCreator(new int[] { HelperFunctions.layers["Level"] }));
        RaycastHit2D topLeft = Physics2D.Raycast(new Vector3(startPos.x * -size.x, startPos.y * size.y, startPos.z), dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.LayerMaskCreator(new int[] { HelperFunctions.layers["Level"] }));
        RaycastHit2D bottomLeft = Physics2D.Raycast(new Vector3(startPos.x * -size.x, startPos.y * -size.y, startPos.z), dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.LayerMaskCreator(new int[] { HelperFunctions.layers["Level"] }));
        RaycastHit2D bottomRight = Physics2D.Raycast(new Vector3(startPos.x * size.x,startPos.y * -size.y,startPos.z), dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.LayerMaskCreator(new int[] { HelperFunctions.layers["Level"] }));
        return topRight||topLeft||bottomRight||bottomLeft;
    }
}
