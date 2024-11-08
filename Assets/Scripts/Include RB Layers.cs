using UnityEngine;

public class IncludeRBLayers : MonoBehaviour
{
    [SerializeField] private int[] layerNumbersToExclude = new int[2];
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers = LayerMaskCreator(layerNumbersToExclude);
    }

    public void ExcludeLayers()
    {
        rb.excludeLayers = LayerMaskCreator(layerNumbersToExclude);
    }
    public void ExcludeAttacks(int[] layersToExclude) {
        rb.excludeLayers = LayerMaskCreator(new int[] { layersToExclude[0], layersToExclude[1], 8});
    }
    private LayerMask LayerMaskCreator(int[] layers) {
        LayerMask binaryLayers = 0;
        if (layers.Length > 0) {
            for (int i = 0; i < layers.Length; i++) {
                binaryLayers += 1 << layers[i];
            }
        }
        return binaryLayers;
    }
}
