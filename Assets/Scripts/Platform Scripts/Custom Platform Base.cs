using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Windows.Speech;

public abstract class CustomPlatformBase : MonoBehaviour
{
    private Dictionary<Rigidbody2D,Rigidbody2D> rbsEffected;
    private Rigidbody2D rbEffected;
    protected Vector2 direction;
    private float speed;
    protected float GetSpeed() => speed;
    protected void ChangeSpeed(float newSpeed) { 
        speed = newSpeed;
        ForceAppliedToUnits(direction, speed);
    }

    public virtual void StayOnCustomPlatform(Rigidbody2D rb) {
        //Debug.Log("Stay Platform");
    }
    public virtual void EnterOnCustomPlatform(Rigidbody2D rb) {
        rbsEffected.Add(rb, rb);
        rbEffected = rb;
    }
    public virtual void ExitOnCustomPlatform(Rigidbody2D rb) {
        rbsEffected.Remove(rb);
        rbEffected = null;
    }

    protected void ForceAppliedToUnits(Vector2 dir, float force = 1) {
        Unit unitEffected = rbEffected?.gameObject.GetComponent<Unit>();
        if (unitEffected != null) {
            rbEffected.linearVelocity -= unitEffected.forceAdded;
            unitEffected.forceAdded = force * dir;
        }
        
    }
}
