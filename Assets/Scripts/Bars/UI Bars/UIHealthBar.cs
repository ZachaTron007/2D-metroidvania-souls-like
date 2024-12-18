using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIHealthBar : BarMethods
{
    private void Awake() {
        bar = GetComponent<RectTransform>();
        barSize = bar.sizeDelta;
        SetUpVars();
    }
    protected override void SetUpVars() {
        base.SetUpVars();
    }

    private void Update() {
        barSize = bar.sizeDelta;
        bar.sizeDelta = ChangeToHealth(bar.sizeDelta);
    }
    
}
