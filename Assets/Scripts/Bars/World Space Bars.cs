using UnityEngine;

public class WorldSpaceBars : BarMethods
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 startingSize;
    private float leftPos;
    [SerializeField]
    private Transform baseTransform;
    

    protected override void SetUpVars() {
        if (!baseTransform) {
            baseTransform = transform;
        }
        barSize = baseTransform.localScale;
        startingSize = barSize;
        leftPos = baseTransform.localPosition.x - baseTransform.localScale.x/2;
        
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
            if (width.x < 0) {
                width.x = 0;
            }
            transform.transform.localScale = new Vector3(width.x, transform.localScale.y, transform.localScale.z);
            transform.localPosition = new Vector3(leftPos+width.x/2, transform.localPosition.y, transform.localPosition.z);

        }
    }
}
