using UnityEngine;

public class WorldSpaceBars : BarMethods
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void SetUpVars() {
        bar = GetComponent<RectTransform>();
        barSize = transform.localScale;
        base.SetUpVars();
    }

    // Update is called once per frame
    protected void UpdateSize()
    {
        barSize = transform.localScale;
        ChangeSquareWidth(ChangeToHealth(barSize));
    }
    private void ChangeSquareWidth(Vector2 width) {
        if (width.x != transform.localScale.x) {
            
            transform.position = new Vector3(transform.position.x - (transform.localScale.x - width.x) / 2, transform.position.y, transform.position.z);
            transform.transform.localScale = new Vector3(width.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
