using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public GameObject lobbyUI;
    private string[] joysticks;
    private static bool[] readyStatus = new bool[4];

    // Start is called before the first frame update
    void Start()
    {
        //readyStatus = new bool[4];
        lobbyUI = GameObject.Find("LobbyUI");
        joysticks = Input.GetJoystickNames();
    }

    // Update is called once per frame
    void Update() {
        CheckReadyStatus();
        CheckForReturnToMainMenu();
        CheckForGameStart();
    }

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

    void ToggleReady(int playerID) {
        Transform playerUI = lobbyUI.transform.Find("Player"+playerID.ToString());
        readyStatus[playerID] = !readyStatus[playerID];
        if (readyStatus[playerID]){
            // Player has readied up, change their text
            playerUI.GetComponentInChildren<Text>().text = "READY";
        }
        else{
            // Player has cancelled ready
            playerUI.GetComponentInChildren<Text>().text = "Press A to ready up";
        }
    }

    void CheckForGameStart() {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7)) {
            int readyCount = 0;
            for(int i = 0; i < 4; ++i) {
                if (readyStatus[i]) {
                    ++readyCount;
                }
            }
            if(readyCount > 1) {
                // Start game
                Debug.Log("Game Started");
                SceneManager.LoadScene("Kerry Test Scene");
                
            }
        }
    }

    void CheckForReturnToMainMenu() {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1)) {
            Debug.Log("Returning to main menu");
            //SceneManager.LoadScene("MainMenu");
        }
    }

    public static bool[] GetReadyStatus() {
        return readyStatus;
    }

    public static void ClearReadyStatus() {
        for(int i = 0; i < 4; ++i) {
            readyStatus[i] = false;
        }
    }


}
