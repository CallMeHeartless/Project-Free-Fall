using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    // Test variables, delete for final build
    [Header("Arena Test Variables")]
    public bool TestMode = false;
    public int TestPlayerCount = 4;



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
            
        } else {

        }
    }

    private void FourWaySplit() {

    }

    private void TwoWaySplit() {

    }

}
