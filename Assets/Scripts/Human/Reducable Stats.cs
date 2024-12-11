using System;
using UnityEngine;

public class ReducableStats : MonoBehaviour {
    public float MAX_VALUE = 100;
    public float currentValue;
    public event Action<bool, float> UpdateValue;
    public event Action MaxValueReached;
    public bool canChange = true;


    public void ChangeCurrentValue(float value) {
        if (canChange) {
            currentValue += value;
            if (currentValue >= MAX_VALUE) {
                currentValue = MAX_VALUE;
                MaxValueReached?.Invoke();
            }
            UpdateValue?.Invoke(true, value);
        }
        
    }
    public void SetCurrentValue(float value) {
        currentValue = value;
        if (currentValue >= MAX_VALUE) {
            currentValue = MAX_VALUE;
            MaxValueReached?.Invoke();
        }
    }

    protected void MaxValue() {
        MaxValueReached?.Invoke();
    }

}
