using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TempHealthBarScript : HealthBarMethods {
    
    [SerializeField] private float ReduceHearBarTime;
    private bool gotHit = false;
    private float counter = 0;
    private int amountChanged = 0;
    private void Awake() {
        health.hitEvent += invoky;
        setUpVars();
    }
    private void invoky(bool hit, int amountChanged) {
        gotHit = hit;
        this.amountChanged = amountChanged;
    }
     private void Update() {
        if (gotHit) {
            counter += Time.deltaTime;
            if (counter > ReduceHearBarTime) {
                changeHealthByNumber(true, amountChanged);
                counter = 0;
                gotHit = false;
            }
        }
        healthBar.sizeDelta = ChangeToHealth();
        //healthBar.sizeDelta = new Vector2(newWidth, healthBar.rect.height);;
    }

}
