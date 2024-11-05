using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TempHealthBarScript : HealthBarMethods {
    
    [SerializeField] private float ReduceHearBarTime;
    private delegate void  HealthBarDelegate(bool hit, int amountChanged);
    private HealthBarDelegate healthBarDelegate;
    private void Awake() {
        healthBarDelegate = changeHealthByNumber;
        health.hitEvent += invoky;
        setUpVars();
    }
    private void invoky(bool hit, int amountChanged) {
        this.Invoke("StartHealthDepletion", ReduceHearBarTime);
    }
    private void StartHealthDepletion() {
        Debug.Log("starting health depletion");
        time = startTime;
    }
     private void Update() {
        healthBar.sizeDelta = ChangeToHealth();
        //healthBar.sizeDelta = new Vector2(newWidth, healthBar.rect.height);;
    }

}
