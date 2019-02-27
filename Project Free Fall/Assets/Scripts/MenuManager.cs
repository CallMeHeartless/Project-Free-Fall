using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        mainMenuUI.transform.GetChild(0).GetComponent<Button>().Select();      
  
    }

    private void Update() {

    }

    public void Play() {
        SceneManager.LoadScene("PlayerReadyLobby");
    }

	public void QuitGame() { 
		Debug.Log("WE QUIT THE GAME!");
		Application.Quit();
	}

    void UpdateButtonSelection() {
        Debug.Log("Current selection: " + selection);
        mainMenuUI.transform.GetChild(selection).GetComponent<Button>().Select();
        
        // Audio
    }


    public void ToggleInstructions() {
        helpActive = !helpActive;
        helpMenu.SetActive(helpActive);
        mainMenuUI.SetActive(!helpActive);
        if (helpActive) {
            helpMenu.GetComponentInChildren<Button>().Select();
        } else {
            mainMenuUI.transform.GetChild(1).GetComponent<Button>().Select();
        }
    }
}