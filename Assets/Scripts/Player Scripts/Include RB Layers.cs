using UnityEngine;

public class IncludeRBLayers : MonoBehaviour
{
    [SerializeField] private int[] layers;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.excludeLayers = LayerMaskCreator(layers);
    }

    


}
