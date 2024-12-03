using UnityEngine;

public class StunBar : WorldSpaceBars
{
    [SerializeField] private Stun stun;
    private void Start()
    {
        stun.stunUpdate += ChangeHealthByNumber;

        SetUpVars();
        valueToWidth = barSize.x / stun.MAX_STUN;
    }

    private void Update()
    {
        UpdateSize();
    }
}
