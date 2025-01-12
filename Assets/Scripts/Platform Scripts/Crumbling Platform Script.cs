using TMPro.Examples;
using UnityEngine;

public class CrumblingPlatformScript : CustomPlatformBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private BoxCollider2D collider;
    private SpriteRenderer sr;
    [SerializeField] private float totalCrumbleTime = 2;
    private float crumbleTime;
    private bool crumbling;
    private float returnCounter;
    [SerializeField] private float returnTime = 4;
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (crumbleTime > 0) {
            crumbleTime -= Time.deltaTime;
            Vector4 color = new Vector4(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(1, 0, 1 - (crumbleTime / totalCrumbleTime)));
            sr.color = color;
        } else if(crumbling) {
            collider.enabled = false;
            returnCounter += Time.deltaTime;
            if (returnCounter >= returnTime) {
                
                collider.enabled = true;
                returnCounter = 0;
                crumbling = false;
                sr.color = new Vector4(sr.color.r, sr.color.g, sr.color.b, 255);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collider) {
        //Debug.Log(collider.gameObject.name);
        if (collider.gameObject.layer == HelperFunctions.layers["Player"]) {
            if (!crumbling) {
                crumbleTime = totalCrumbleTime;
            }
            crumbling = true;
        }
    }

    private void Crumbling() {
        crumbling = false;
    }
}
