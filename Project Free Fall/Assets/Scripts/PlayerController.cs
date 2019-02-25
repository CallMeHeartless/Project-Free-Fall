using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region PlayerVariables
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
    [SerializeField]
    private float sideDashForce;
    [SerializeField]
    private float sideDashCooldown;
    private float sideDashTimer = 0.0f;
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
    //[SerializeField]
    //private float maxKnockbackMultiplier = 4.0f;
    [SerializeField]
    private float[] knockbackMultiplier;
    private int knockbackIndex = 0;
    [SerializeField][Tooltip("The number of hits needed to break armour")]
    private int knockbackIncrementThreshold;
    private int knockbackDamageCount = 0;

    private combat.CurrentAction currentState = combat.CurrentAction.move;

    // Other components
    private Animator anim;
    private DashHitboxController dashController;

    #endregion

    void Start(){
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        dashController = transform.Find("DashHitBox").GetComponent<DashHitboxController>();

        // DEBUG - assign IDs for test level
        if(GameObject.Find("RoundManager") == null) {
            AssignPlayerID(playerID); // Change this later
        }

    }

    void Update() {
        movement = Vector3.zero;
        if(currentState == combat.CurrentAction.stun) {
            return;
        }

        MovementInput();
        RotatePlayer();
        Jump();
        SideDash();

        if (Input.GetButtonDown(playerR1Button)) {
            BasicAttack();
        }

        if (Input.GetButton(playerL1Button)) {
            ChargeDash();
            movement *= 0.25f;
        }
        else if (Input.GetButtonUp(playerL1Button)) {
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
            rb.MovePosition(transform.position + moveSpeed * movement * Time.fixedDeltaTime * knockbackMultiplier[knockbackIndex]);
        }
    }

    // Rotates the player (and camera)
    void RotatePlayer() {
        transform.Rotate(transform.up, turnSpeed * Input.GetAxis(playerRightXAxis) * knockbackMultiplier[knockbackIndex]);
    }

    // Adds an impulse to the player (such as from a knock back effect)
    public void AddImpulse(Vector3 impulse) {
        rb.AddForce(impulse * knockbackMultiplier[knockbackIndex], ForceMode.Impulse);
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
            // Play charge audio
        }

        dashChargeTimer += Time.deltaTime;
        if(dashChargeTimer >= maxDashChargeTime) {
            PerformDash();
        }
    }

    // Execute Dash
    void PerformDash() {
        // Animation
        anim.SetBool("Dash", false);
        // Audio - stop charge, play dash

        // Set dash strength
        float dashMagnitude = minChargeForce + maxChargeForce * (dashChargeTimer / maxDashChargeTime);
        dashController.SetForceStrength(dashMagnitude);
        
        // Push player forward 
        //if(Input.GetAxis(playerLeftXAxis) != 0.0f) {
        //    AddImpulse(transform.right * dashMagnitude * Input.GetAxisRaw(playerLeftXAxis));
        //} else {
        //    AddImpulse(transform.forward * dashMagnitude);
        //}
        AddImpulse(transform.forward * dashMagnitude);

        // Reset charge
        dashChargeTimer = 0.0f;
        StopAfterDelay(0.5f);
    }

    IEnumerator StopAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        StopPlayer();
    }
    
    public void StopPlayer() {
        rb.velocity = Vector3.zero;
    }

    public void StunPlayer(float stunDuration) {
        if(currentState == combat.CurrentAction.stun) {
            return;
        }
        // Stun player
        StopPlayer();
        currentState = combat.CurrentAction.stun;
        // Visual/Audio
        StartCoroutine(RemoveStun(stunDuration));
    }

    IEnumerator RemoveStun(float stunDuration) {
        yield return new WaitForSeconds(stunDuration);
        // Remove stun status 
        currentState = combat.CurrentAction.move;
        // Visual / Audio
    }

    void BasicAttack() {
        anim.SetTrigger("Attack");
    }

    void SideDash() {
        sideDashTimer += Time.deltaTime;
        if(Input.GetButtonDown(playerBButton) && sideDashTimer >= sideDashCooldown) {
            // Dash
            float direction = Input.GetAxisRaw(playerLeftXAxis);
            if(direction == 0.0f) {
                AddImpulse(transform.forward * -1.0f * sideDashForce);
            } else {
                AddImpulse(transform.right * sideDashForce * Input.GetAxisRaw(playerLeftXAxis));
                Debug.Log(transform.right * sideDashForce * Input.GetAxisRaw(playerLeftXAxis));
            }
            // Animation?

            sideDashTimer = 0.0f;
        }
    }

    // Increment recorded hits against the player, breaking armour if a certain threshold is reached
    public void DamagePlayer(int damageIncrement) {
        if(knockbackIndex >= 6) {
            return;
        }
        knockbackDamageCount += damageIncrement;
        Debug.Log(knockbackDamageCount);
        if(knockbackDamageCount >= knockbackIncrementThreshold) {
            // Break armour
            ++knockbackIndex;
            knockbackDamageCount = 0;
            Debug.Log("Player " + playerID.ToString() + " force multiplier: " + knockbackMultiplier[knockbackIndex].ToString());
        }
    }

}
