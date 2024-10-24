using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Sensors : MonoBehaviour {

    public event Action <Collider2D> triggerEnter;
    public event Action <Collider2D> triggerStay;
    public event Action <Collider2D> triggerExit;
    public BoxCollider2D hitBox;

    public event Action <Collision2D> collisionEnter;

    private void Awake() {
        hitBox = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        triggerEnter?.Invoke(other);
    }
    private void OnTriggerStay2D(Collider2D other) {
        triggerStay?.Invoke(other);
    }
    private void OnTriggerExit2D(Collider2D other) {
        triggerExit?.Invoke(other);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        collisionEnter?.Invoke(other);
    }
}
