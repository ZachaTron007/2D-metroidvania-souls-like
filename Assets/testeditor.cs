using UnityEngine;

public class testeditor : MonoBehaviour
{
#if UNITY_EDITOR
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("test");
    }
#endif

    // Update is called once per frame
    void Update()
    {
        
    }
}
