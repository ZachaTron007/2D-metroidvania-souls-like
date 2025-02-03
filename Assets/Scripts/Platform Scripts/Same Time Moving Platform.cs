using UnityEngine;

public class SameTimeMovingPlatform : MovingPlatformScript
{
    protected override void Awake() {
        base.Awake();
        totalTime = moveSpeed;
    }
}
