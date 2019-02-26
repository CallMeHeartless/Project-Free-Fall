using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject mainMenuUI;
    public GameObject helpMenu;
    private int selection = 0;
    private int maxSelection = 2;
    private bool helpActive = false;
    private bool input = false;

    private void Start() {
        mainMenuUI = GameObject.Find("MainMenu");
        UpdateButtonSelection();
        //GameObject helpMenu = GameObject.Find("ControlsMenu");
    }

    private void Update() {

        Debug.Log("Axis: " + Input.GetAxisRaw("Controller_0_Left_Y_Axis"));
        // Switch between selected buttons
        if (Input.GetAxisRaw("Controller_0_Left_Y_Axis") == 1.0f && !input) {

            input = true;
            ++selection;
            if(selection > maxSelection) {
                selection = 0;
            }
            UpdateButtonSelection();
        }
        else if(Input.GetAxisRaw("Controller_0_Left_Y_Axis") == -1.0f && !input) {

            input = true;
            --selection;
            if(selection < 0) {
                selection = maxSelection;
            }
            UpdateButtonSelection();
        } 
        else {
            input = false;
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
        //Debug.Log(selection);
        // Audio
    }

    void TriggerSelection() {
        mainMenuUI.transform.GetChild(selection).GetComponent<Button>().onClick.Invoke();
        // Audio
    }

    public void ToggleInstructions() {
        helpActive = !helpActive;
        helpMenu.gameObject.SetActive(helpActive);
    }
}