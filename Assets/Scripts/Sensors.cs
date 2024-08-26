using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sensors : MonoBehaviour {

    public UnityEvent<Collider2D> triggerEnter;
    public UnityEvent<Collider2D> triggerExit;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter2D(Collider2D other) {
        triggerEnter?.Invoke(other);
    }
    private void OnTriggerExit2D(Collider2D other) {
        triggerExit?.Invoke(other);
    }

}
