using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class Health1 : MonoBehaviour {
    private PlayerState player;

    [SerializeField] private int health = 100;
    private int MAX_HEALTH = 100;
    private Vector4 hurtColor = new Vector4(255, 62, 62, 255);
    private Vector4 normalColor;
    private float colorChangeSpeed = 0.7f;
    private SpriteRenderer sr;
    [SerializeField] private float parryWindow = 5f;
    private int blockButton = 1;
    [SerializeField] private bool blocking = false;
    [SerializeField] private bool parrying = false;
    
    private const string BLOCKING = "blocking";
    private const string BLOCKPARRY = "blockParry";

    // Update is called once per frame
    private void Awake() {
        player = GetComponent<PlayerState>();
        health = MAX_HEALTH;
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
        hurtColor /= 255;
    }

    void Update() {

        if (health <= 0) {
            die();
        }
    }


    public void Damage(int damage) {
        if (damage > 0) {
            health -= damage;
            if (health < 0) {
                health = 0;
            }
            StartCoroutine(Hurt(sr));
        }

    }
    public void Heal(int heal) {
        if (heal > 0) {
            health += heal;
            if (health > MAX_HEALTH) {
                health = MAX_HEALTH;
            }
        }

    }

    IEnumerator Hurt(SpriteRenderer sr) {
        sr.color = hurtColor;
        yield return new WaitForSeconds(colorChangeSpeed);
        sr.color = normalColor;
        yield return null;
    }

    public void die() {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
       if (player.state!=player.blockState) {
            if (collision.GetComponent<DamageScript>() != null) {
                DamageScript damageScript = collision.GetComponent<DamageScript>();
                Damage(damageScript.damage);

            }
        }
    }
    /*
    public void Block(Animator animatior) {
        
            animatior.SetBool(BLOCKING, true);
            //float parryWindow = .5f;
            Debug.Log("Blocking");
            parrying = true;
            Invoke("ParryWindowEnd", parryWindow);
            blocking = true;

    }
    public void StopBlocking(Animator animatior) {
        if (Input.GetMouseButtonUp(blockButton)) {
            animatior.SetBool(BLOCKING, false);
            blocking = false;

        }
    }
    private void ParryWindowEnd() {
        parrying = false;
    }*/
}
