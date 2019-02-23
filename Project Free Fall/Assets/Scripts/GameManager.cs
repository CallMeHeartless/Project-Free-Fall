using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static bool[] readyStatus = new bool[4];
    private static int[] playerScores = new int[4];
    private static bool inGame = false;

    // Ensure that only one instance of the game manager exists, and that it persists between scenes
    private void Awake() {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("GameManager");
        if(managers.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()    {
       
        //if(instance == null) {
        //    instance = this;
        //}
    }
    
    void Update()    {

    }



    // Checks to see if only one player remains alive
    private bool CheckForLastStanding() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length == 1;
    }

    public static bool[] GetReadyStatus() {
        return readyStatus;
    }

    public static void ClearReadyStatus() {
        for (int i = 0; i < 4; ++i) {
            readyStatus[i] = false;
        }
    }

    static void ClearPlayerScores() {
        for(int i = 0; i < 4; ++i) {
            playerScores[i] = 0;
        }
    }

    public static void SetPlayerCount(int _count) {
        for(int i = 0; i < _count; ++i) {
            readyStatus[i] = true;
        }
    }

}
