using UnityEngine;

public class IncludeRBLayers : MonoBehaviour
{
    [SerializeField] private int[] layers;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers = LayerMaskCreator(layers);
    }

    public LayerMask LayerMaskCreator(int[] layers) {
        LayerMask binaryLayers = 0;
        if (layers.Length > 0) {
            for (int i = 0; i < layers.Length; i++) {
                binaryLayers += 1 << layers[i];
            }
        }
        return binaryLayers;
    }


}
