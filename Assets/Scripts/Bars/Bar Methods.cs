using UnityEditor;
using UnityEngine;

public class BarMethods : MonoBehaviour
{
    protected RectTransform bar;
    [SerializeField] protected float totalTime;
    protected float time = 0;
    protected float maxWidth = 200;
    protected float valueToWidth;
    protected float newWidth = 200;
    protected float oldWidth = 200;
    protected float height = 1;
    protected Vector2 barSize;
    protected virtual void SetUpVars() {
        
        maxWidth = barSize.x;
        height = barSize.y;
        oldWidth = maxWidth;
        newWidth = maxWidth;
    }
    public virtual void ChangeHealthToNumber(bool hit, float amountChanged) {
        ChangeToHealth(amountChanged);
    }
    public void ChangeHealthByNumber(bool hit, DamageScript enemyAttack) {
        UpdateHealth(hit, -enemyAttack.damage);

    }
    public void ChangeHealthByNumber(bool hit,  float amountChanged = 0) {
        UpdateHealth(hit, amountChanged);

    }

    public void ChangeHealthByPercent(bool hit, float amountChanged) {
        //UpdateHealth(hit,(amountChanged / 100) * health.MAX_HEALTH);
    }
    public void ChangeHealthToPercent(bool hit, float amountChanged) {
        //ChangeToHealth((amountChanged / 100) * health.MAX_HEALTH);
    }

    private void UpdateHealth(bool hit, float value) {
        time = totalTime;
        oldWidth = barSize.x;
        Debug.Log("width: " + value + ", on " + gameObject.name);
        newWidth = barSize.x + value * valueToWidth;
        if (newWidth >= maxWidth) {
            newWidth = maxWidth;
        }
    }
    protected Vector2 ChangeToHealth(Vector2 healthBarSize) {
        if (time > 0) {
            time -= Time.deltaTime;
            float thing = Mathf.Lerp(oldWidth, newWidth, 1 - (time / totalTime));
            healthBarSize = new Vector2(thing, height);
            
        }
        return healthBarSize;
    }
    private void ChangeToHealth(float value) {
        barSize = new Vector2(value * valueToWidth, height);
        if (barSize.x >= maxWidth) {
            barSize = new Vector2(maxWidth, height);
        }
    }

}