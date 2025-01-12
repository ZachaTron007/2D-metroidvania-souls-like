using UnityEngine;

public class CustomPlatformBase : MonoBehaviour
{
    public virtual void StayOnCustomPlatform(Rigidbody2D rb) {
        //Debug.Log("Stay Platform");
    }
    public virtual void EnterOnCustomPlatform(Rigidbody2D rb) {
        //Debug.Log("Enter Platform");
    }
    public virtual void ExitOnCustomPlatform(Rigidbody2D rb) {
        //Debug.Log("Left Platform");
    }
}
