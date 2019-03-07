using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public GameObject lobbyUI;
    private string[] joysticks;

    // Start is called before the first frame update
    void Start() {
        lobbyUI = GameObject.Find("LobbyUI");
        joysticks = Input.GetJoystickNames();
        // Clear old data
        GameManager.ClearPlayerScores();
        GameManager.ClearReadyStatus();
    }

    // Update is called once per frame
    void Update() {
        CheckReadyStatus();
        CheckForReturnToMainMenu();
        CheckForGameStart();
        if (Input.GetButtonDown("StartGame")) {
            Debug.Log("the start button is fucking working");
        }
    }

    // Checks for input from each controller signalling that player is ready / not ready
    void CheckReadyStatus() {
        if (Input.GetButtonDown("Controller_0_A")) {
            ToggleReady(0);
        }
        if (Input.GetButtonDown("Controller_1_A")) {
            ToggleReady(1);
        }
        if (Input.GetButtonDown("Controller_2_A")) {
            ToggleReady(2);
        }
        if (Input.GetButtonDown("Controller_3_A")) {
            ToggleReady(3);
        }
    }

    // Toggle a player's ready status
    void ToggleReady(int playerID) {
        
        Transform playerUI = lobbyUI.transform.Find("Player"+playerID.ToString());
        GameManager.readyStatus[playerID] = !GameManager.readyStatus[playerID];
        if (GameManager.readyStatus[playerID]){
            // Player has readied up, change their text
            playerUI.GetComponentInChildren<Text>().text = "READY";
            // Display character
            playerUI.GetChild(0).gameObject.SetActive(true);
        }
        else{
            // Player has cancelled ready
            playerUI.GetComponentInChildren<Text>().text = "Press A to ready up";
            // Hide character
            playerUI.GetChild(0).gameObject.SetActive(false);
        }

        // Adjust UI
        if (CheckIfReady()) {
            lobbyUI.transform.GetChild(1).gameObject.SetActive(true);
        } else {
            lobbyUI.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Starts the game if at least two players are ready and one presses start
    void CheckForGameStart() {
        if (Input.GetButtonDown("StartGame") && CheckIfReady()) {
                SceneManager.LoadScene("lvl_Arena_One");
        }
    }

    // Checks for input to return to the main menu
    void CheckForReturnToMainMenu() {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Checks if at least two players are ready, returning true or false accordingly
    bool CheckIfReady() {
        int readyCount = 0;
        bool[] playersReady = GameManager.GetReadyStatus();
        for (int i = 0; i < 4; ++i) {
            if (playersReady[i]) {
                ++readyCount;
            }
        }

        return readyCount > 1;
    }

}
