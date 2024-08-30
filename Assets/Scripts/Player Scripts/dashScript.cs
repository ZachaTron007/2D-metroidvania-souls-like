using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dashScript : MonoBehaviour
{
    private float dashSpeed = 15;
    private float dashduration = 0.2f;
    public bool dashing;

    void Start() {
    }

    public IEnumerator dash(float direction,Rigidbody2D rb) {
        Invoke("dashReset", dashduration);
        dashing = true;
        while (dashing) {
            rb.velocity = Vector2.right * direction * dashSpeed;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    private void dashReset() {
        dashing = false;
    }
    
}
