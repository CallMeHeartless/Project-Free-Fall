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
    private float dashCooldownTimer = 0.0f;
    //[SerializeField]
    //private float maxKnockbackMultiplier = 4.0f;
    [SerializeField]
    private float[] knockbackMultiplier;
    private int knockbackIndex = 0;
    [SerializeField][Tooltip("The number of hits needed to break armour")]
    private int knockbackIncrementThreshold;
    private int knockbackDamageCount = 0;

    [Header("Misc")]
    [SerializeField]
    private float victoryOrbWinTime;
    private float victoryOrbTimer = 0.0f;

    private combat.CurrentAction currentState = combat.CurrentAction.move;
    private bool hasVictoryOrb = false;

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

        if (hasVictoryOrb) {
            ProcessVictoryOrb();
        }

        MovementInput();
        RotatePlayer();
        Jump();
        SideDash();

        if (Input.GetButtonDown(playerR1Button)) {
            BasicAttack();
        }

        if(dashCooldownTimer > 0) {
            dashCooldownTimer -= Time.deltaTime;
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
        if(dashCooldownTimer > 0.0f) {
            return;
        }

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
        dashCooldownTimer = dashCooldown;
        StopAfterDelay(0.5f);
    }

    // Stops the player after a short delay - used to terminate the charged dash
    IEnumerator StopAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        StopPlayer();
    }
    
    // Makes the velocity of the player's RigidBody component zero
    public void StopPlayer() {
        rb.velocity = Vector3.zero;
    }

    // Sets the player's state to be stunned, forcing it to skip its update step. Auto resolved by coroutine
    public void StunPlayer(float stunDuration) {
        if(currentState == combat.CurrentAction.stun) {
            return;
        }
        // Stun player
        StopPlayer();
        currentState = combat.CurrentAction.stun;
        // Visual/Audio
        StartCoroutine(RemoveStun(stunDuration));

        // Drop orb if they have it
        if (hasVictoryOrb) {
            RemoveVictoryOrb();
        }
    }

    // Removes the stun status of the player after a set duration
    IEnumerator RemoveStun(float stunDuration) {
        yield return new WaitForSeconds(stunDuration);
        // Remove stun status 
        currentState = combat.CurrentAction.move;
        // Visual / Audio
    }

    // Triggers the sword attack animation
    void BasicAttack() {
        anim.SetTrigger("Attack");
    }

    // Applies a short impulse to the player, backwards with no input or to either side
    void SideDash() {
        sideDashTimer += Time.deltaTime;
        if(Input.GetButtonDown(playerBButton) && sideDashTimer >= sideDashCooldown) {
            // Dash
            float direction = Input.GetAxisRaw(playerLeftXAxis);
            if(direction == 0.0f) {
                AddImpulse(transform.forward * -1.0f * sideDashForce);
                // Backdash animation
            } 
            else {
                AddImpulse(transform.right * sideDashForce * Input.GetAxisRaw(playerLeftXAxis));
                if(direction > 0.0f) {
                    anim.SetTrigger("DodgeRight");
                } 
                else {
                    anim.SetTrigger("DodgeLeft");
                }
            }

            // Dash Audio

            sideDashTimer = 0.0f;
        }
    }

    // Increment recorded hits against the player, breaking armour if a certain threshold is reached
    public void DamagePlayer(int damageIncrement) {
        // Drop orb if they have it
        if (hasVictoryOrb) {
            RemoveVictoryOrb();
        }

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

    // Returns true if the player is flagged as possessing the victory orb
    public bool CheckForVictoryOrb() {
        return hasVictoryOrb;
    }

    // Flags the player as having the victory orb
    public void GiveVictoryOrb() {
        hasVictoryOrb = true;
    }

    // Removes the victory orb from the player, spawning a new one and resetting their timer
    public void RemoveVictoryOrb() {
        hasVictoryOrb = false;
        //RoundManager.SpawnVictoryOrb(); // Change this to control where the orb spawns
        GameObject victoryOrb = Instantiate(Resources.Load("VictoryOrb", typeof(GameObject))) as GameObject;
        if (victoryOrb) {
            victoryOrb.transform.position = transform.position + Vector3.up * 7.5f;
            Vector3 direction = new Vector3(Random.Range(1.5f, 3.5f), Random.Range(3.5f, 6.5f), Random.Range(1.5f, 3.5f));
            victoryOrb.GetComponent<Rigidbody>().AddForce(3.0f * direction, ForceMode.Impulse);
        } else {
            Debug.LogError("ERROR: Path to victory orb could not be found");
        }
        victoryOrbTimer = 0.0f;
    }

    // Tracks the player's progress towards winning with the orb - calling for the round to end if they have won
    void ProcessVictoryOrb() {
        victoryOrbTimer += Time.deltaTime;
        if(victoryOrbTimer >= victoryOrbWinTime) {
            GameObject roundManager = RoundManager.GetManager();
            if (roundManager) {
                roundManager.GetComponent<RoundManager>().VictoryOrbWin(playerID);
                victoryOrbTimer = -1000.0f;
            }
        }
    }

}
