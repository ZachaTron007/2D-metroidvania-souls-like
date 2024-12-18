using UnityEngine;

public class WorldSpaceBars : BarMethods
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float leftPos;
    [SerializeField]
    private Transform baseTransform;

    private void Start() {
        SetUpVars();
    }
    protected override void SetUpVars() {
        if (!baseTransform) {
            baseTransform = transform;
        }
        barSize = baseTransform.localScale;
        leftPos = baseTransform.localPosition.x - baseTransform.localScale.x/2;
        
        base.SetUpVars();
    }

    // Update is called once per frame
    protected void Update()
    {
        //if (transform.localScale.x != 0) {
            barSize = transform.localScale;
        //}
        ChangeSquareWidth(ChangeToHealth(barSize));
    }
    private void ChangeSquareWidth(Vector2 width) {
        if (width.x != transform.localScale.x) {
            if (width.x < 0) {
                //Debug.Log("Negative width");
                width.x = 0;
            }
            //Debug.Log(width.x+", On "+gameObject.name);
            transform.transform.localScale = new Vector3(width.x, transform.localScale.y, transform.localScale.z);
            transform.localPosition = new Vector3(leftPos+width.x/2, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
