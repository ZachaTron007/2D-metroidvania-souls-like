using UnityEngine;

public class LerpingInhertedClass : State
{
    [Header("Lerp Settings")]
    [SerializeField] protected float totalTime;
    
    [SerializeField] protected AnimationCurve curve;
    public virtual void FinishedLerping() {

    }
}
