using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject MainMenuUI;
    private int selection = 0;

    private void Start() {
        MainMenuUI = GameObject.Find("MainMenu");
        MainMenuUI.transform.GetChild(selection).GetComponent<Button>().Select();
    }

    void Update() {
        if (Input.GetButtonDown("Controller_0_Left_Y_Axis")) {

        }
    }


    public void Play()
    {
        SceneManager.LoadScene(1);
    }

	public void QuitGame()
	{
		Debug.Log("WE QUIT THE GAME!");
		Application.Quit();
	}

    private void UpdateSelectButton() {
        MainMenuUI.transform.GetChild(selection).GetComponent<Button>().Select();
    }
}