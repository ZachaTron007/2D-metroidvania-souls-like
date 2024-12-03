using System;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public event Action stunEvent;
    public event Action<bool, float> stunUpdate;
    public float MAX_STUN = 5;
    public float currentStunLevel;
    [SerializeField] private float stunReduceSpeed = .5f;
    [SerializeField] private float resetStunTime = 1;
    private float time;
    private Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health = GetComponent<Health>();
        health.hitEvent += TakeStun;
        currentStunLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStunLevel > 0) {
            time += Time.deltaTime;
            if (time > resetStunTime) {
                changeStun(Time.deltaTime * -stunReduceSpeed);
            }
        } else {
            currentStunLevel = 0;
            time = 0;
        }
    }

    private void TakeStun(bool hit, DamageScript EnemyAttack) {
        if(currentStunLevel >= MAX_STUN ) {
            stunEvent?.Invoke();
        } else {
            stunUpdate?.Invoke(hit, (int)EnemyAttack.stun);
            changeStun(EnemyAttack.stun);
            time = 0;
        }

    }

    private void changeStun(float value) {
        stunUpdate?.Invoke(true, -value);
        currentStunLevel += value;
    }
}
