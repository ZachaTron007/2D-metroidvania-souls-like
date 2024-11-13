using UnityEngine;

public class OneWayPassThroughScript : MonoBehaviour
{
    [SerializeField] private Sensors playerSensor;
    private BoxCollider2D mainCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCollider = GetComponent<BoxCollider2D>();
        playerSensor.triggerEnter += PlayerUnder;
        playerSensor.triggerExit += PlayerNotUnder;
    }

    private void PlayerUnder(Collider2D collider) {
        if (collider.gameObject.layer == 3) {
            Debug.Log("Player is Under");
            mainCollider.excludeLayers = 1 << 3;
        }
    }
    private void PlayerNotUnder(Collider2D collider) {
        if (collider.gameObject.layer == 3) {
            Debug.Log("Player is not Under");
            mainCollider.excludeLayers = 0;
        }
    }
}
