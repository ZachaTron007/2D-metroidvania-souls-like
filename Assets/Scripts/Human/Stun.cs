using System;
using UnityEngine;

public class Stun : ReducableStats
{
    [SerializeField] private float stunReduceSpeed = 5f;
    [SerializeField] private float resetStunTime = 1;
    private float time;
    private Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health = GetComponent<Health>();
        health.hitEvent += TakeStun;
        currentValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentValue > 0) {
            time += Time.deltaTime;
            if (time > resetStunTime) {
                float stunReduce = Time.deltaTime * -stunReduceSpeed;
                //Debug.Log("Reducing stun by: "+stunReduce);
                ChangeCurrentValue(stunReduce);
            }
        } else {
            SetCurrentValue(0);
            time = 0;
        }
    }

    public void TakeStun(bool hit, DamageScript EnemyAttack) {
        if(currentValue >= MAX_VALUE ) {
            MaxValue();
            ChangeCurrentValue(-1);
        } else {
            ChangeCurrentValue(EnemyAttack.stun);
            time = 0;
        }

    }
}
