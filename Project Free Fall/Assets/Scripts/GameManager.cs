using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()    {
        InitialisePlayers();
        LobbyController.ClearReadyStatus();// Change this to occur when the game is over
    }

    // Update is called once per frame
    void Update()    {
        
    }

    void InitialisePlayers() {
        // Iterate through registered ready players and ready prefab instances for them
        bool[] playersInGame = LobbyController.GetReadyStatus();
        for(int i = 0; i < 4; ++i) {
            // Currently assuming players exist in level prior to game
            GameObject player = GameObject.Find("Player" + i.ToString());
            if (playersInGame[i]) {
                // Assign id to player instance
                player.GetComponent<PlayerController>().AssignPlayerID(i);
            } else {
                if(player != null) {
                    Destroy(player);
                }
            }
        }

        
    }

}
