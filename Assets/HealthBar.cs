using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthBar : HealthBarMethods
{

    private void Awake() {
        health.hitEvent += changeHealthByNumber;
        setUpVars();
    }

    private void Update() {
        healthBar.sizeDelta = ChangeToHealth();
    }
    
}
