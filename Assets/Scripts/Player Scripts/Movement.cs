using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    private PlayerControls playerControls;
    private InputAction move;
    //colliders
    [SerializeField] private Sensors groundSensor;
    [SerializeField] private Sensors rightWallSensor;
    [SerializeField] private Sensors leftWallSensor;
    //movements
    [SerializeField] private float moveSpeed = 250;
    private float direction = 1;
    private int dashDirection = 1;
    //Jumps
    [SerializeField] private float fallGravMultiplier = 3f;
    [SerializeField] private float jumpGravMultiplier = 20f;
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpHeight = 3;
    private float terminalVelocity = 15;
    


    //components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveVetcor = new Vector2(0,0);


    /*FUNCTIONS*/
   

    
    
    //DAMAGE VISUAL
    

    


    
    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
    }
    private void OnDisable() {
        move = playerControls.Player.Move;
        move.Disable();
    }
}
