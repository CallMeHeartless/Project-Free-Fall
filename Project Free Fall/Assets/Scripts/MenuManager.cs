using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

	public void QuitGame()
	{
		Debug.Log("WE QUIT THE GAME!");
		Application.Quit();
	}
}