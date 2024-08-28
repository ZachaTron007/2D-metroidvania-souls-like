using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashScript : MonoBehaviour
{
    private float dashSpeed = 6;
    private float dashduration = 0.13f;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator dash(float dashduration,int direction, bool dashing) {
        Invoke("dashReset", dashduration);
        
        while (dashing) {
            rb.velocity += Vector2.right * dashSpeed * direction;
            yield return null;
        }
        yield return null;
    }
    private void dashReset() {
        //dashing = false;
    }
}
