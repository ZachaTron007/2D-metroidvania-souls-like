using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TempHealthBarScript : BarMethods {
    
    [SerializeField] private float ReduceHearBarTime;
    [SerializeField] private Health health;
    private bool gotHit = false;
    private float counter = 0;
    private int amountChanged = 0;
    private void Awake() {
        health.hitEvent += invoky;
        bar = GetComponent<RectTransform>();
        barSize = bar.sizeDelta;
        SetUpVars();
    }
    private void invoky(bool hit, DamageScript enemyAttack) {
        gotHit = hit;
        amountChanged = -enemyAttack.damage;
    }
     private void Update() {
        if (gotHit) {
            counter += Time.deltaTime;
            if (counter > ReduceHearBarTime) {
                
                ChangeHealthByNumber(true, amountChanged: amountChanged);
                counter = 0;
                gotHit = false;
            }
        }
        barSize = bar.sizeDelta;
        bar.sizeDelta = ChangeToHealth(bar.sizeDelta);
        //healthBar.sizeDelta = new Vector2(newWidth, healthBar.rect.height);
    }

}
