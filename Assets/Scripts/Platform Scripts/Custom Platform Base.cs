using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Windows.Speech;

public abstract class CustomPlatformBase : MonoBehaviour
{
    private List<Rigidbody2D> rbsEffected = new List<Rigidbody2D> { };
    private Rigidbody2D rbEffected;
    protected Vector2 direction;
    private float speed;
    protected float GetSpeed() => speed;
    protected void ChangeSpeed(float newSpeed) { 
        speed = newSpeed;
        ForceAppliedToUnits(direction, speed);
    }

    public void StayOnCustomPlatform(Rigidbody2D rb) {
        
    }
    public void EnterOnCustomPlatform(Rigidbody2D rb) {
        //rbEffected = rb;
        rbsEffected.Add(rb);
        ForceAppliedToUnits(direction, speed);
    }
    public void ExitOnCustomPlatform(Rigidbody2D rb) {
        rbsEffected.Remove(rb);
        rbEffected = null;
    }

    protected void ForceAppliedToUnits(Vector2 dir, float force = 1) {
        Debug.Log(rbsEffected);
        foreach (Rigidbody2D rb in rbsEffected) {
            Unit unitEffected = null;
            rb.gameObject.TryGetComponent(out unitEffected);
            rb.linearVelocity -= unitEffected.forceAdded;
            unitEffected.forceAdded = force * dir;
        }

    }
}
