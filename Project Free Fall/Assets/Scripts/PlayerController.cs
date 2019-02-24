using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Individual player controller input identifiers
    [SerializeField]
    private int playerID;
    private string playerLeftXAxis;
    private string playerLeftYAxis;
    private string playerRightXAxis;
    private string playerAButton;
    private string playerBButton;
    private string playerL1Button;
    private string playerR1Button;
    private string playerTriggers;

    
    [Header("Movement")] // Player movement variables
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float jumpForce;
    private Rigidbody rb;
    private Vector3 movement;

    [Header("Combat")]
    [SerializeField]
    private float maxDashChargeTime = 2.5f;
    private float dashChargeTimer = 0.0f;
    [SerializeField]
    private float maxChargeForce = 20.0f;
    [SerializeField]
    private float minChargeForce = 2.0f;
    [SerializeField]
    private float dashCooldown = 1.0f;
    private bool canDash = true;
    [SerializeField]
    private float minimumMass = 1.0f;


    // Other components
    private Animator anim;
    private DashHitboxController dashController;

    void Start(){
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        dashController = transform.Find("DashHitBox").GetComponent<DashHitboxController>();

        AssignPlayerID(playerID); // Change this later
    }

    void Update() {
        movement = Vector3.zero;
        MovementInput();
        RotatePlayer();
        Jump();

        if (Input.GetButton(playerR1Button)) {
            ChargeDash();
        }
        else if (Input.GetButtonUp(playerR1Button)) {
            PerformDash();
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
        playerAButton = "Controller_" + _playerID.ToString() + "_A";
        playerBButton = "Controller_" + _playerID.ToString() + "_B";
        playerL1Button = "Controller_" + _playerID.ToString() + "_L1";
        playerR1Button = "Controller_" + playerID.ToString() + "_R1";
        playerTriggers = "Controller_" + _playerID.ToString() + "_L2R2";
    }

    // Obtain the player ID
    public int GetPlayerID() {
        return playerID;
    }

    // Gets player input and sets corresponding animation
    void MovementInput(){
        Vector3 right = transform.right * Input.GetAxis(playerLeftXAxis);
        Vector3 forward = transform.forward * Input.GetAxis(playerLeftYAxis);
        movement = right + forward;

        // Movement animation
        if(anim != null) {
            anim.SetFloat("VelocityRight", movement.x);
            anim.SetFloat("VelocityForward", movement.z);
        }

    }

    // Moves the player according to the player's basic input
    void MoveBody() {
        if(movement != Vector3.zero) {
            rb.MovePosition(transform.position + moveSpeed * movement * Time.fixedDeltaTime);
        }
    }

    // Rotates the player (and camera)
    void RotatePlayer() {
        transform.Rotate(transform.up, turnSpeed * Input.GetAxis(playerRightXAxis));
    }

    // Adds an impulse to the player (such as from a knock back effect)
    public void AddImpulse(Vector3 impulse) {
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    // Basic jump - gives the player a slight grace if they have only just started falling
    void Jump() {
        if(Input.GetButtonDown(playerAButton) && rb.velocity.y <= 0.0f && rb.velocity.y >= -0.01f) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Begin dash sequence
    void ChargeDash() {
        // Animation
        if (!anim.GetBool("Dash")) {
            anim.SetBool("Dash", true);
        }

        dashChargeTimer += Time.deltaTime;
        if(dashChargeTimer >= maxDashChargeTime) {
            Debug.Log("Force Dash");
            PerformDash();
        }
    }

    // Execute Dash
    void PerformDash() {
        // Animation
        anim.SetBool("Dash", false);

        // Set dash strength - SEND TO DASH BOX
        float dashMagnitude = minChargeForce + maxChargeForce * (dashChargeTimer / maxDashChargeTime);
        dashController.SetForceStrength(dashMagnitude);
        // DEBUG
        AddImpulse(transform.forward * dashMagnitude);

        // Reset charge
        dashChargeTimer = 0.0f;
        StopAfterDelay(0.75f);
    }

    IEnumerator StopAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        StopPlayer();
    }
    
    public void StopPlayer() {
        rb.velocity = Vector3.zero;
    }

    public void StunPlayer(float stunDuration) {
        // Stun player
        StopPlayer();
        StartCoroutine(RemoveStun(stunDuration));
    }

    IEnumerator RemoveStun(float stunDuration) {
        yield return new WaitForSeconds(stunDuration);
        // Remove stun status 
    }

}
