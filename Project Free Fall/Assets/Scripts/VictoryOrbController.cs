using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryOrbController : MonoBehaviour
{
    public static bool isCollected = false;

    private void Start() {
        isCollected = false;
    }

    // The orb can be picked up when the player runs into it
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !isCollected) {
            isCollected = true; // Block other players from collecting the orb
            collision.gameObject.GetComponent<PlayerController>().GiveVictoryOrb();
            // Audio
            Destroy(gameObject);
        }
    }
}
