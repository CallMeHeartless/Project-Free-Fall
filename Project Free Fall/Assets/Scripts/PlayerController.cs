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
    private string playerBackButton;

    
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
    [SerializeField][Tooltip("Coefficient for speed bonus from losing armour: 1.0f for same as force knockback")]
    private float armourLossSpeedDamping = 0.3f;
    private Rigidbody rb;
    private Vector3 movement;
    [SerializeField]
    private float victoryOrbSpeedPenalty = 0.9f;
    [SerializeField][Tooltip("The speed the player must be moving at to trigger their dash trails")]
    private float trailSpeedThreshold = 10.0f;

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
    [SerializeField]
    private float[] knockbackMultiplier;
    private int knockbackIndex = 0;
    [SerializeField][Tooltip("The number of hits needed to break armour")]
    private int knockbackIncrementThreshold;
    private int knockbackDamageCount = 0;
    [SerializeField][Tooltip(" ")]
    GameObject[] armourComponents;


    [Header("Misc")]
    [SerializeField]
    private float victoryOrbWinTime;
    private float victoryOrbTimer = 0.0f;
    public ParticleSystem[] chargeThrusters;
    public ParticleSystem[] dashThrusters;
    public Light[] cooldownLights;
    public GameObject victoryOrbLight;

    private combat.CurrentAction currentState = combat.CurrentAction.move;
    private bool hasVictoryOrb = false;

    // Other components
    private Animator anim;
    private DashHitboxController dashController;
    private spawnScore scoreReference;

    #endregion

    void Start(){
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        UpdateCrestColour();
        // DEBUG - assign IDs for test level
        if (GameObject.Find("RoundManager") == null) {
            AssignPlayerID(playerID); // Change this later
        }

        dashController = transform.Find("DashHitBox").GetComponent<DashHitboxController>();
        GameObject inGameScore = GameObject.Find("InGameScoreUI");
        
        if(inGameScore == null) {
            Debug.LogError("ERROR: InGameScoreUI null reference exception");
        } else {
            scoreReference = inGameScore.GetComponent<spawnScore>();
        }

        
    }

    void Update() {
        movement = Vector3.zero;

        // Check for stun
        if(currentState == combat.CurrentAction.stun) {
            return;
        }

        // Check for victory orb progress
        if (hasVictoryOrb) {
            ProcessVictoryOrb();
        }

        // Process player input commands
        MovementInput();
        RotatePlayer();
        //Jump();
        SideDash();

        // Handle basic attack
        if (Input.GetButtonDown(playerR1Button)) {
            BasicAttack();
        }

        // Handle dash cooldown
        if(dashCooldownTimer < dashCooldown) {
            dashCooldownTimer += Time.deltaTime;

        } 
        else {
            if (!hasVictoryOrb) {
                // Handle charge dash
                if (Input.GetButton(playerL1Button)) {
                    ChargeDash();
                    movement *= 0.25f;
                } else if (Input.GetButtonUp(playerL1Button)) {
                    PerformDash();
                }
                ToggleCooldownLight(0, true);
            }

        }

        // Toggle score
        if(Input.GetButtonDown(playerBackButton) || Input.GetButtonUp(playerBackButton)) {
            ToggleInGameScore();
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
        playerBackButton = "Controller_" + _playerID.ToString() + "_Back";
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
            if (hasVictoryOrb) {
                movement *= victoryOrbSpeedPenalty;
            }
            float moveModifier = Mathf.Max(1.0f, armourLossSpeedDamping * knockbackMultiplier[knockbackIndex]);
            rb.MovePosition(transform.position + moveSpeed * movement * moveModifier * Time.fixedDeltaTime);
            
        }
    }

    // Rotates the player (and camera)
    void RotatePlayer() {
        float turnMultiplier = Mathf.Max(1.0f, armourLossSpeedDamping * knockbackMultiplier[knockbackIndex]);
        transform.Rotate(transform.up, turnSpeed * turnMultiplier * Input.GetAxis(playerRightXAxis));
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
        if(dashCooldownTimer < dashCooldown) {
            return;
        }

        // Animation
        if (!anim.GetBool("Dash")) {
            anim.SetBool("Dash", true);
            // Play charge audio
            transform.GetChild(4).gameObject.GetComponent<PlayerAudioController>().PlayerChargingAudio();

        }
        // VFX
        ToggleChargeThrusters(true);

        // Add time to the charge up, triggering it if maximum charge has been reached
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
        transform.GetChild(4).gameObject.GetComponent<PlayerAudioController>().StopPlayerChargingAudio();
        transform.GetChild(4).gameObject.GetComponent<PlayerAudioController>().PlayerdashingAudio();

        transform.GetChild(2).gameObject.SetActive(true);
        //Debug.Log(transform.GetChild(2).gameObject.name);
        // VFX
        ToggleChargeThrusters(false);
        ToggleDashThrusters(true);
        ToggleCooldownLight(0, false);

        // Set dash strength
        float dashMagnitude = minChargeForce + maxChargeForce * (dashChargeTimer / maxDashChargeTime);
        dashController.SetForceStrength(dashMagnitude);
        
        // Push player forward 
        AddImpulse(transform.forward * dashMagnitude);

        // Reset charge
        dashChargeTimer = 0.0f;
        dashCooldownTimer = 0.0f;
        StartCoroutine(StopAfterDelay(0.5f));
    }

    // Stops the player after a short delay - used to terminate the charged dash
    IEnumerator StopAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        transform.GetChild(2).gameObject.SetActive(false);
        StopPlayer();
    }
    
    // Makes the velocity of the player's RigidBody component zero
    public void StopPlayer() {
        rb.velocity = Vector3.zero;
        movement = Vector3.zero;
        // Cull any thruster effects
        ToggleDashThrusters(false);
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
        anim.SetTrigger("Damage");

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
        if(sideDashTimer >= sideDashCooldown) {
            if (Input.GetButtonDown(playerBButton)) {
                // Dash
                float direction = Input.GetAxisRaw(playerLeftXAxis);
                if (direction == 0.0f) {
                    AddImpulse(transform.forward * -1.0f * sideDashForce);
                    // Backdash animation
                } else {
                    AddImpulse(transform.right * sideDashForce * Input.GetAxisRaw(playerLeftXAxis));
                    if (direction > 0.0f) {
                        anim.SetTrigger("DodgeRight");
                    } else {
                        anim.SetTrigger("DodgeLeft");
                    }
                }

                // Dash Audio

                // VFX
                ToggleDashThrusters(true);
                ToggleCooldownLight(1, false);
                StartCoroutine(CullSideDashParticles());

                sideDashTimer = 0.0f;

            } 
            else {
                ToggleCooldownLight(1, true);
            }
        }
    }

    IEnumerator CullSideDashParticles() {
        yield return new WaitForSeconds(0.5f);
        ToggleDashThrusters(false);
    }

    // Increment recorded hits against the player, breaking armour if a certain threshold is reached
    public void DamagePlayer(int damageIncrement) {
        // Drop orb if they have it
        if (hasVictoryOrb) {
            RemoveVictoryOrb();
        }

        // Animation
        anim.SetTrigger("Damage");

        if(knockbackIndex >= 6) {
            return;
        }
        knockbackDamageCount += damageIncrement;

        if(knockbackDamageCount >= knockbackIncrementThreshold) {
            // Break armour
            DetachArmour();
            ++knockbackIndex;
            knockbackDamageCount = 0;
            //Debug.Log("Player " + playerID.ToString() + " force multiplier: " + knockbackMultiplier[knockbackIndex].ToString());
        }
    }

    // Returns true if the player is flagged as possessing the victory orb
    public bool CheckForVictoryOrb() {
        return hasVictoryOrb;
    }

    // Flags the player as having the victory orb
    public void GiveVictoryOrb() {
        hasVictoryOrb = true;
        ToggleVictoryOrbLight(true);

    }

    // Removes the victory orb from the player, spawning a new one and resetting their timer
    public void RemoveVictoryOrb() {
        hasVictoryOrb = false;
        ToggleVictoryOrbLight(false);
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

    // Plays the charge thruster particle system if true, stops if false
    private void ToggleChargeThrusters(bool on) {
        if (chargeThrusters != null) {
            foreach (ParticleSystem thruster in chargeThrusters) {
                if (!thruster.isPlaying && on) {
                    thruster.Play();
                }
                else if(thruster.isPlaying && !on) {
                    thruster.Stop();
                }
            }
        }
    }

    // Plays the dash thrusters if true, stops them if false
    private void ToggleDashThrusters(bool on) {
        if (dashThrusters != null) {
            foreach (ParticleSystem thruster in dashThrusters) {
                if (!thruster.isPlaying && on) {
                    thruster.Play();
                } else if (thruster.isPlaying && !on) {
                    thruster.Stop();
                }
            }
        }
    }

    // Handles the series of armour being removed
    private void DetachArmour() {
        /* Add to prefab in this order
         * Left shin + left thigh (0, 1)
         * Left Shoulder (2)
         * Right shin + right thigh (3, 4)
         * Right Arm (5, 6, 7)
         * Chest (8)
         * Crest (9)
         */

        // Update crest colour
        UpdateCrestColour();

        // Skip this if all armour is lost or no armour exists
        if (knockbackIndex > 5) {
            return;
        }

        // Remove armour accordingly
        switch (knockbackIndex) {
            case 0: {
                GameObject leftThigh = armourComponents[0];
                if (leftThigh) {
                    DetachSingleArmourPiece(leftThigh);
                }
                GameObject leftShin = armourComponents[1];
                if (leftShin) {
                    DetachSingleArmourPiece(leftShin);
                }
                    
                break;
            }

            case 1: {
                GameObject leftShoulder = armourComponents[2];
                if (leftShoulder) {
                    DetachSingleArmourPiece(leftShoulder);
                }
                break;
            }

            case 2: {
                GameObject rightShin = armourComponents[3];
                if (rightShin) {
                    DetachSingleArmourPiece(rightShin);
                }
                GameObject rightThigh = armourComponents[4];
                if (rightThigh) {
                    DetachSingleArmourPiece(rightThigh);
                }
                break;
            }

            case 3: {
                GameObject rightShoulder = armourComponents[5];
                if (rightShoulder) {
                    DetachSingleArmourPiece(rightShoulder);
                }

                GameObject rightArm = armourComponents[6];
                if (rightArm) {
                    DetachSingleArmourPiece(rightArm);
                }
                GameObject rightBracer = armourComponents[7];
                if (rightBracer) {
                    DetachSingleArmourPiece(rightBracer);
                }
                break;
            }

            case 4: {
                GameObject chest = armourComponents[8];
                if (chest) {
                    DetachSingleArmourPiece(chest);
                }
                break;
            }

            case 5: {
                GameObject crest = armourComponents[9];
                if (crest) {
                    DetachSingleArmourPiece(crest);
                }
                break;
            }

            default:break;
        }
    }

    // Removes a single piece of armour, giving it a collider and rigidbody
    private void DetachSingleArmourPiece(GameObject armour) {
        // Remove from character
        armour.transform.SetParent(null);

        // Add collider
        armour.AddComponent<BoxCollider>();

        // Add rigidbody
        Rigidbody armourRB = armour.AddComponent<Rigidbody>();
        Vector3 detachForce = new Vector3(Random.Range(1.0f, 3.0f), Random.Range(2.0f, 5.0f), Random.Range(1.0f, 3.0f));
        Vector3 spinForce = new Vector3(Random.Range(1.0f, 3.0f), Random.Range(2.0f, 5.0f), Random.Range(1.0f, 3.0f));
        armourRB.AddForce(detachForce * 4.0f, ForceMode.Impulse);
        armourRB.AddTorque(spinForce * 20.0f, ForceMode.Impulse);

        // Trigger self-destruct timer on armour
        armour.GetComponent<ArmourController>().StartSelfDestructTimer();
    }

    // Updates the colour of the crest to visually represent how vulnerable the player is
    private void UpdateCrestColour() {
        if(armourComponents[9] == null) {
            Debug.LogError("ERROR: Crest null reference");
            return;
        }

        Material crestMaterial = armourComponents[9].GetComponent<MeshRenderer>().material;
        float damageRatio = 1.0f - ((float)knockbackIndex / 6.0f);
        crestMaterial.SetFloat("_HealthLevel", damageRatio);        
    }

    // Turns the in game score UI for that player on or off
    private void ToggleInGameScore() {
        scoreReference.seeScore(playerID);
    }

    // Turns a light corresponding to the cooldown for player abilities on or off
    private void ToggleCooldownLight(int abilityIndex, bool state) {
        if (cooldownLights[abilityIndex] != null && cooldownLights.Length > abilityIndex) {
            if (cooldownLights[abilityIndex].gameObject.activeSelf == state) {
                return;
            }
            cooldownLights[abilityIndex].gameObject.SetActive(state);
        }
    }

    // Turns on the effects that show a player has the victor orb
    private void ToggleVictoryOrbLight(bool on) {
        if(victoryOrbLight != null) {
            if(victoryOrbLight.gameObject.activeSelf == on) {
                return;
            }
            victoryOrbLight.gameObject.SetActive(on);
        }
    }
}
