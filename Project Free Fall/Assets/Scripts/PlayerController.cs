using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Individual player control variables
    [SerializeField]
    private int playerID;
    private string playerLeftXAxis;
    private string playerLeftYAxis;
    private string playerRightXAxis;

    // Player movement variables
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;
    private Rigidbody rb;
    private Vector3 movement;

    //Animation controller. adam
    private Animator anim;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        AssignPlayerID(playerID); // Change this later

        
    }

    // Update is called once per frame
    void Update() {
        movement = Vector3.zero;
        MovementInput();
        RotatePlayer();
        
        if (Input.GetKey(KeyCode.UpArrow)) {// Change input key
            anim.SetTrigger("Dash");
        }
        
    }

    void FixedUpdate() {
        MoveBody();
    }

    // Sets the player ID - this is essential for differentiating players and controller input
    public void AssignPlayerID(int _playerID) {
        playerID = _playerID;
        Debug.Log("Assigned ID " + _playerID.ToString() + " to player");
        playerLeftXAxis = "Controller_" + _playerID.ToString() + "_Left_X_Axis";
        playerLeftYAxis = "Controller_" + _playerID.ToString() + "_Left_Y_Axis";
        playerRightXAxis = "Controller_" + _playerID.ToString() + "_Right_X_Axis";

    }

    // Gets player input and sets corresponding animation
    void MovementInput(){
        Vector3 right = transform.right * Input.GetAxis(playerLeftXAxis);
        Vector3 forward = transform.forward * Input.GetAxis(playerLeftYAxis);
        movement = right + forward;
        // Movement animation
        if(anim != null) {
            anim.SetFloat("VelocityRight", movement.x);
            anim.SetFloat("VelocityForward", movement.y);
        }

    }

    // Moves the player according to the player's basic input
    void MoveBody() {
        if(movement != Vector3.zero) {
            rb.MovePosition(transform.position + moveSpeed * movement * Time.fixedDeltaTime);
        }
        Debug.Log(movement);
    }

    // Rotates the player (and camera)
    void RotatePlayer() {
        transform.Rotate(transform.up, turnSpeed * Input.GetAxis(playerRightXAxis));
    }

    // Adds an impulse to the player (such as from a knock back effect)
    public void AddImpulse(Vector3 impulse) {
        rb.AddForce(impulse, ForceMode.Impulse);
    }

}
