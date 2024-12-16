using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private float layer;
    private float paralaxModifier;
    private SpriteRenderer sr;
    private void Awake() {
        paralaxModifier = (layer != 0) ? 0.2f * layer : 1;
        sr = GetComponent<SpriteRenderer>();
        layer = sr.sortingOrder;
        //paralaxModifier = (layer != 0) ? layer : 1;
    }
    void Update()
    {
        Vector2 newSpeed = ParalaxManager.instance.speed * paralaxModifier;
        transform.position += new Vector3(newSpeed.x, newSpeed.y,0);
        
    }
}
