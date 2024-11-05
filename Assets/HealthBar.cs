using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthBar : MonoBehaviour
{
    protected RectTransform healthBar;
    [SerializeField] protected Health health;
    [SerializeField] protected float startTime;
    [SerializeField] protected float time;
    private float maxWidth;
    private float healthToWidth;
    protected float newWidth = 200;
    protected float oldWidth;
    private void Awake() {
        health.hitEvent +=changeHealthByNumber;
        healthBar = GetComponent<RectTransform>();
        maxWidth = healthBar.rect.width;
        newWidth = maxWidth;
        healthToWidth = maxWidth / health.MAX_HEALTH;
    }
    public void changeHealthToNumber(bool hit,int amountChanged) {
        ChangeToHealth(amountChanged);
    }
    public void changeHealthByNumber(bool hit, int amountChanged) {
        UpdateHealth(hit,amountChanged);
    }

    public void changeHealthByPercent(bool hit, float amountChanged) {
        //UpdateHealth(hit,(amountChanged / 100) * health.MAX_HEALTH);
    }
    public void changeHealthToPercent(bool hit, float amountChanged) {
        ChangeToHealth((amountChanged / 100) * health.MAX_HEALTH);
    }

    protected void UpdateHealth(bool hit, int value) {
        time = startTime;
        oldWidth = healthBar.sizeDelta.x;
        newWidth = healthBar.sizeDelta.x + value * healthToWidth;
        
        //healthBar.sizeDelta = new Vector2(thing, healthBar.rect.height);
        if (newWidth >= maxWidth) {
            newWidth = maxWidth;
        }
    }
    protected virtual void Update() {
        healthBar.sizeDelta = ChangeToHealth();
    }
    protected Vector2 ChangeToHealth() {
        if (time > 0) {
            time -= Time.deltaTime;
            float thing = Mathf.Lerp(oldWidth, newWidth, 1 - (time / startTime));
            Vector2 healthBarSize = new Vector2(thing, healthBar.rect.height);
            return healthBarSize;
        }
        return healthBar.sizeDelta;
    }
    private void ChangeToHealth(float value) {
        healthBar.sizeDelta = new Vector2(value*healthToWidth, healthBar.rect.height);
        if (healthBar.sizeDelta.x >= maxWidth) {
            healthBar.sizeDelta = new Vector2(maxWidth, healthBar.rect.height);
        }
    }
}
