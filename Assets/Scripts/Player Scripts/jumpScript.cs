using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpScript : MonoBehaviour
{
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    private float terminalVelocity = 15;
    public bool grounded = false;
    // Start is called before the first frame update
    void Start()
    {
        jumpVelocity = Mathf.Sqrt(Physics.gravity.y * 2 * jumpHeight * -2);
    }

    // Update is called once per frame
    void Update()
    {
             
    }

    public void Jump(Rigidbody2D rb) {
        //wallSliding = false;
        //makes the y component change
        grounded = false;
        rb.velocity = Vector2.up * jumpVelocity;
        rb.gravityScale = 2;
    }

}
