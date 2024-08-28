using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class bulletScript : MonoBehaviour
{
    Vector2 dir;
    [SerializeField] private float speed = 1;
    [SerializeField] private float despawnRate = 1;
    [SerializeField] private float counter;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player;
        player = GameObject.FindWithTag("Player");
        dir = player.transform.position-transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.Euler(0, 0, vectorAngle(dir) + 180);
        Debug.Log("Angle: "+vectorAngle(dir));
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(dir*speed*Time.deltaTime);
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        counter += Time.deltaTime;
        if (counter > despawnRate)
            Destroy(gameObject);

    }

    public float vectorAngle(Vector2 dir) {
        dir.Normalize();
        int sign = (dir.y >= 0) ? 1 : -1;
        //int xSign = (dir.x >= 0) ? 1 : -1;
        int offset = (sign >= 0) ? 0 : 360;
        Debug.Log("Offset: " + offset);
        //Debug.Log("sign: " + sign);
        float ang = Vector2.Angle(Vector2.up, dir) * sign + offset + 180;//new Vector2(dir.x * xSign, dir.y * ySign));
        return ang;
    }
}
