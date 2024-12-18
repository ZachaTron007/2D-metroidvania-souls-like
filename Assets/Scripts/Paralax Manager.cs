using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ParalaxManager : MonoBehaviour
{
    public static ParalaxManager instance;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera vcam;
    [HideInInspector] public Vector2 speed;
    [SerializeField] private float paralaxMultiplier = 1;
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        speed = (vcam.transform.position - transform.position)*paralaxMultiplier;
        transform.position = new Vector3(vcam.transform.position.x, vcam.transform.position.y, transform.position.z);
    }
}
