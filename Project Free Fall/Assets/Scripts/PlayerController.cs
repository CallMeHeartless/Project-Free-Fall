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
    private string playerAButton;

    // Player movement variables
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float jumpForce;
    private Rigidbody rb;
    private Vector3 movement;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        AssignPlayerID(playerID); // Change this later
    }

    // Update is called once per frame
    void Update(){
        movement = new Vector3();
        MovementInput();
        RotatePlayer();
    }

    void FixedUpdate() {
        MoveBody();
        Jump();
    }

    public void AssignPlayerID(int _playerID) {
        playerID = _playerID;
        Debug.Log("Assigned ID " + _playerID.ToString() + " to player");
        playerLeftXAxis = "Controller_" + _playerID.ToString() + "_Left_X_Axis";
        playerLeftYAxis = "Controller_" + _playerID.ToString() + "_Left_Y_Axis";
        playerRightXAxis = "Controller_" + _playerID.ToString() + "_Right_X_Axis";
        playerAButton = "Controller_" + _playerID.ToString() + "_A";

    }

    void MovementInput(){
        Vector3 right = transform.right * Input.GetAxis(playerLeftXAxis);
        Vector3 forward = transform.forward * Input.GetAxis(playerLeftYAxis);
        movement = right + forward;
    }

    void MoveBody(){
        if(movement != Vector3.zero) {
            rb.MovePosition(transform.position + moveSpeed * movement * Time.fixedDeltaTime);
        }

    }

    void RotatePlayer(){
        transform.Rotate(transform.up, turnSpeed * Input.GetAxis(playerRightXAxis) * Time.deltaTime);
    }

    void Jump() {
        if (Input.GetButtonDown(playerAButton) && rb.velocity.y == 0) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void AddImpulse(Vector3 impulse) {
        rb.AddForce(impulse, ForceMode.Impulse);
    }
}
