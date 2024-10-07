using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animatior;
    public int direction { get; protected set; } = 1;
    public float moveSpeed { get; protected set; } = 250;
    public float attackTime;
    public bool grounded { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
