using UnityEngine;
using UnityEngine.Rendering;

public class FlyingIdelState : BaseIdelState
{
    [SerializeField] private Vector3 point;
    private BoxCollider2D box;
    [SerializeField] private Sensors hitBoxScript;
    [SerializeField] private float idelSpeed;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private GameObject tempMarker;
    [SerializeField] private Vector2 dir;
    private float timer;
    bool arrived = false;
    public override void Enter()
    {
        animator.Play(idelAniamtion.name);
        PointPicker();
    }

    private Vector3 PointPicker() {
        point = new Vector3(Random.Range(hitBoxScript.hitBox.bounds.min.x, hitBoxScript.hitBox.bounds.max.x), Random.Range(hitBoxScript.hitBox.bounds.min.x, hitBoxScript.hitBox.bounds.max.y), unitVariables.transform.position.z);
        dir = (point - unitVariables.transform.position).normalized;
        Debug.DrawRay(unitVariables.transform.position, dir, Color.red, 1f);
        while (ShootCheckRay()) {
            Debug.Log(Physics2D.Raycast(unitVariables.transform.position, dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.layers["Level"]));
            point = new Vector3(Random.Range(hitBoxScript.hitBox.bounds.min.x, hitBoxScript.hitBox.bounds.max.x), Random.Range(hitBoxScript.hitBox.bounds.min.x, hitBoxScript.hitBox.bounds.max.y), unitVariables.transform.position.z);
            Debug.DrawRay(unitVariables.transform.position, dir, Color.red, .3f);
            dir = (point - unitVariables.transform.position).normalized;
        }
        arrived = false;
        Instantiate(tempMarker, point, Quaternion.identity);
        return point;
    }

    public override void UpdateState() {
        
        if (!arrived) {
            Debug.Log("point: " + point);
            Debug.Log("dir: " + dir);
            Debug.Log("Flip?: " + (point.x < unitVariables.transform.position.x));
            unitVariables.sr.flipX = point.x < unitVariables.transform.position.x;
            rb.linearVelocity = dir * idelSpeed * Time.deltaTime;
            if (Vector2.Distance(unitVariables.transform.position, point) < 0.1f) {
                arrived = true;
            }
        } else {
            arrived = true;
            unitVariables.transform.position = point;
            rb.linearVelocity = Vector2.zero;
            timer +=Time.deltaTime;
            if (timer > waitTime) {
                PointPicker();
                timer = 0;
            }
            //Invoke(nameof(PointPicker), waitTime);
            
        }
    }
    private bool ShootCheckRay() {
        RaycastHit2D hit = Physics2D.BoxCast(unitVariables.transform.position, unitVariables.mainCollider.hitBox.size,0,dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.layers["Level"]);

        RaycastHit2D ray1 = Physics2D.Raycast(unitVariables.transform.position, dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.layers["Level"]);
        RaycastHit2D ray2 = Physics2D.Raycast(unitVariables.transform.position, dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.layers["Level"]);
        RaycastHit2D ray3 = Physics2D.Raycast(new Vector3(unitVariables.transform.position.x, unitVariables.transform.position.y, unitVariables.transform.position.z), dir, HelperFunctions.PointToDistance(transform.position, point), HelperFunctions.layers["Level"]);

        return hit;
    }
}
