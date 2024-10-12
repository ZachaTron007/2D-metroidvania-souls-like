using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sensors : MonoBehaviour {

    public event Action <Collider2D> triggerEnter;
    public event Action<Collider2D> triggerStay;
    public event Action<Collider2D> triggerExit;
    private void OnTriggerEnter2D(Collider2D other) {
        triggerEnter?.Invoke(other);
    }
    private void OnTriggerStay2D(Collider2D other) {
        triggerStay?.Invoke(other);
    }
    private void OnTriggerExit2D(Collider2D other) {
        triggerExit?.Invoke(other);
    }

}
