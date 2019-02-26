using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject mainMenuUI;
    private int selection = 0;
    private int maxSelection = 2;

    private void Start() {
        mainMenuUI = GameObject.Find("MainMenu");
        UpdateButtonSelection();
    }

    private void Update() {
        // Switch between selected buttons
        if(Input.GetAxisRaw("Controller_0_Left_Y_Axis") == 1.0f) {
            ++selection;
            if(selection > maxSelection) {
                selection = 0;
            }
            UpdateButtonSelection();
        }
        else if(Input.GetAxisRaw("Controller_0_Left_Y_Axis") == -1.0f) {
            --selection;
            if(selection < 0) {
                selection = maxSelection;
            }
            UpdateButtonSelection();
        }

        // User makes a selection
        if (Input.GetButtonDown("Controller_0_A")) {
            TriggerSelection();
        }
    }

    public void Play() {
        SceneManager.LoadScene("PlayerReadyLobby");
    }

	public void QuitGame() { 
		Debug.Log("WE QUIT THE GAME!");
		Application.Quit();
	}

    void UpdateButtonSelection() {
        mainMenuUI.transform.GetChild(selection).GetComponent<Button>().Select();
        // Audio
    }

    void TriggerSelection() {
        mainMenuUI.transform.GetChild(selection).GetComponent<Button>().onClick.Invoke();
        // Audio
    }
}