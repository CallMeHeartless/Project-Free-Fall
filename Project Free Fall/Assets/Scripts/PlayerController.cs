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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AssignPlayerID(playerID); // Change this later
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3();
        MovementInput();
        RotatePlayer();
    }

    void FixedUpdate() {
        MoveBody();
    }

    public void AssignPlayerID(int _playerID)
    {
        playerID = _playerID;
        Debug.Log("Assigned ID " + _playerID.ToString() + " to player");
        playerLeftXAxis = "Controller_" + _playerID.ToString() + "_Left_X_Axis";
        playerLeftYAxis = "Controller_" + _playerID.ToString() + "_Left_Y_Axis";
        playerRightXAxis = "Controller_" + _playerID.ToString() + "_Right_X_Axis";

    }

    void MovementInput()
    {
        Vector3 right = transform.right * Input.GetAxis(playerLeftXAxis);
        Vector3 forward = transform.forward * Input.GetAxis(playerLeftYAxis);
        movement = right + forward;
    }

    void MoveBody()
    {
        rb.AddForce(moveSpeed * movement, ForceMode.VelocityChange);
    }

    void RotatePlayer()
    {
        transform.Rotate(transform.up, turnSpeed * Input.GetAxis(playerRightXAxis));
    }
}
