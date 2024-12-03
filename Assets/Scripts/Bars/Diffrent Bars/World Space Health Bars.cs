using UnityEngine;

public class WorldSpaceHealthBars : WorldSpaceBars
{
    [SerializeField] private Health health;
    void Start()
    {
        health.hitEvent += ChangeHealthByNumber;
        
        SetUpVars();
        valueToWidth = barSize.x / health.MAX_HEALTH;
    }

    private void Update()
    {
        UpdateSize();
    }
}
