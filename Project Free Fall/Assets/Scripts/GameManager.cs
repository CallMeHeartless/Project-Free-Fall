using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static bool[] readyStatus = new bool[4];
    private static int[] playerScores = new int[4];
    public static int winThreshold = 3;

    // Ensure that only one instance of the game manager exists, and that it persists between scenes
    private void Awake() {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("GameManager");
        if(managers.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public static bool[] GetReadyStatus() {
        return readyStatus;
    }

    public static void ClearReadyStatus() {
        for (int i = 0; i < 4; ++i) {
            readyStatus[i] = false;
        }
    }

    public static void ClearPlayerScores() {
        for(int i = 0; i < 4; ++i) {
            playerScores[i] = 0;
        }
    }

    public static void AddToPlayerScore(int player) {
        ++playerScores[player];
    }

    public static void SetPlayerCount(int _count) {
        for(int i = 0; i < _count; ++i) {
            readyStatus[i] = true;
        }
    }

    public static bool CheckForGameOver() {
        for(int i = 0; i < 4; ++i) {
            if(playerScores[i] == winThreshold) {
                return true;
            }
        }

        return false;
    }

    public static int[] GetPlayerScores() {
        return playerScores;
    }

}
