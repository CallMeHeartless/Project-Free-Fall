using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    // Test variables, delete for final build
    [Header("Arena Test Variables")]
    public bool TestMode = false;
    public int TestPlayerCount = 4;

    [Header("Arena Variables")]
    [SerializeField]
    private float ringFallInterval = 25.0f;
    private int ringCount = 1;
    private bool roundWon = false;

    [SerializeField]
    private float roundOverTimeDelay = 3.0f;

    // Start is called before the first frame update
    void Start(){
        // To force testing the arena without going through the full loop
        if (TestMode) {
            GameManager.SetPlayerCount(TestPlayerCount);
        }

        InitialisePlayers();
        InitialiseCameras();
    }

    // Update is called once per frame
    void Update(){

        // End of round check
        if (CheckForLastStanding() && !roundWon) {
            roundWon = true;
            // Award the player one point
            GameManager.AddToPlayerScore(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetPlayerID());

            // Round over text

            // End of Round check
            StartCoroutine(EndOfRound());
        }
    }

    void InitialisePlayers() {
        // Iterate through registered ready players and ready prefab instances for them
        bool[] readyStatus = GameManager.GetReadyStatus();
        for (int i = 0; i < 4; ++i) {
            // Currently assuming players exist in level prior to game
            GameObject player = GameObject.Find("Player" + i.ToString());
            if (readyStatus[i]) {
                // Assign id to player instance
                player.GetComponent<PlayerController>().AssignPlayerID(i);
            } else {
                if (player != null) {
                    Destroy(player);
                }
            }
        }
    }

    void InitialiseCameras() {
        // Obtain number of ready players
        int playerCount = 0;
        bool[] playersReady = GameManager.GetReadyStatus();
        for(int i = 0; i < 4; ++i) {
            if (playersReady[i]) {
                ++playerCount;
            }
        }

        // Define camera setup accordingly
        Transform cameras = GameObject.Find("Cameras").GetComponent<Transform>();
        if(playerCount > 2) {
            cameras.GetChild(0).GetComponent<Camera>().rect = new Rect(new Vector2(-0.5f, 0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(1).GetComponent<Camera>().rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(2).GetComponent<Camera>().rect = new Rect(new Vector2(-0.5f, -0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(3).GetComponent<Camera>().rect = new Rect(new Vector2(0.5f, -0.5f), new Vector2(1.0f, 1.0f));
        } else {
            Destroy(cameras.GetChild(2).GetComponent<GameObject>());
            Destroy(cameras.GetChild(3).GetComponent<GameObject>());
        }
    }

    // Checks to see if only one player remains alive
    private bool CheckForLastStanding() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length == 1;
    }

    // Performs end of round checks, starting a new round or returning to main menu
    IEnumerator EndOfRound() {
        yield return new WaitForSeconds(roundOverTimeDelay);

        if (GameManager.CheckForGameOver()) {
            SceneManager.LoadScene(0);
        } else {
            SceneManager.LoadScene("lvl_Arena_One");
        }
    }

}
