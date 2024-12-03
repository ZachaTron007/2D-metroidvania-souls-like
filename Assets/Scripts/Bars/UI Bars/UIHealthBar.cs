using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIHealthBar : BarMethods
{
    [SerializeField] protected Health health;
    private void Awake() {
        bar = GetComponent<RectTransform>();
        barSize = bar.sizeDelta;
        health.hitEvent += ChangeHealthByNumber;
        SetUpVars();
    }
    protected override void SetUpVars() {
        base.SetUpVars();
        valueToWidth = barSize.x / health.MAX_HEALTH;
    }

    private void Update() {
        barSize = bar.sizeDelta;
        bar.sizeDelta = ChangeToHealth(bar.sizeDelta);
    }
    
}
