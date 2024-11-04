using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Health health;
    [SerializeField] private int maxWidth = 200;
    [SerializeField] private float healthToWidth;
    [SerializeField] private int maxHeight = 40;

    private void Awake() {
        //healthToWidth = health.MAX_HEALTH/maxWidth;
        Debug.Log(health.MAX_HEALTH);
        healthToWidth = maxWidth/ health.MAX_HEALTH;
        Debug.Log(healthToWidth);
        changeHealth(50);
    }
    public void changeHealth(int amountChanged) {
        healthBar.sizeDelta = new Vector2(amountChanged*healthToWidth,maxHeight);
    }
}
