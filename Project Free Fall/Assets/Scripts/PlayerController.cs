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
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AssignPlayerID(playerID); // Change this later

        _animator = GetComponent<Animator>(); //adam
    }

    // Update is called once per frame
    void Update()


    {
        movement = new Vector3();
        MovementInput();
        RotatePlayer();

        //adam
        
            if (_animator == null) return;

            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            Move(x,y);
        
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
        if(movement != Vector3.zero) {
            rb.MovePosition(transform.position + moveSpeed * movement * Time.fixedDeltaTime);
        }

    }

    void RotatePlayer()
    {
        transform.Rotate(transform.up, turnSpeed * Input.GetAxis(playerRightXAxis));
    }

    public void AddImpulse(Vector3 impulse) {
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    //adam
    private void Move(float x, float y)
    {
    _animator.SetFloat("VelocityRight", x);
    _animator.SetFloat("VelocityForward", y);
    }
}
