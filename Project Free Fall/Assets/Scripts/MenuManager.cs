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
    private bool helpActive = false;

    private void Start() {
        //mainMenuUI = GameObject.Find("MainMenu");
        mainMenuUI.transform.GetChild(0).GetComponent<Button>().Select();      
  
    }

    private void Update() {

    }

    public void Play() {
        SceneManager.LoadScene("PlayerReadyLobby");
        GetComponent<AudioSource>().Play(0);
    }

	public void QuitGame() {
        Application.Quit();
        GetComponent<AudioSource>().Play(0);
        
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
        GetComponent<AudioSource>().Play(0);
    }

    public void ReturnToMainMenu() {
        GameObject gameMusic = GameObject.Find("GameMusic");
        if (gameMusic) {
            gameMusic.GetComponent<GameMusicController>().Stop();
            DestroyImmediate(gameMusic);
        }
        SceneManager.LoadScene(0);
    }
}