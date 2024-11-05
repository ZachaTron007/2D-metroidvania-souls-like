using UnityEditor;
using UnityEngine;

public class TempHealthBarScript : HealthBar
{
    
    [SerializeField] private float ReduceHearBarTime;
    [SerializeField] private float time;
    
    private void Awake() {
        healthBar = GetComponent<RectTransform>();
        health.hitEvent += invoky;
        health.hitEvent += UpdateHealth;
        Debug.Log("size: "+healthBar.sizeDelta);
    }
    private void invoky(bool hit, int amountChanged) {
        Invoke("StartHealthDepletion", ReduceHearBarTime);
    }
    private void StartHealthDepletion() {
        Debug.Log("starting health depletion");
        time = startTime;
    }
    override protected void Update() {
        //tempHealthBar.sizeDelta = ChangeToHealths();
        Debug.Log(newWidth);
        healthBar.sizeDelta = new Vector2(newWidth, healthBar.rect.height);;
    }
    private Vector2 ChangeToHealths() {
        if (time > 0) {
            Debug.Log("depleeting");
            time -= Time.deltaTime;
            float thing = Mathf.Lerp(oldWidth, newWidth, 1 - (time / startTime));
            Vector2 healthBarSize = new Vector2(thing, healthBar.rect.height);
            return healthBarSize;
        }
        return healthBar.sizeDelta;
    }
}
