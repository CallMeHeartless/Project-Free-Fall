using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryOrbController : MonoBehaviour
{
    // The orb can be picked up when the player runs into it
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().GiveVictoryOrb();
            // Audio
            DestroyImmediate(gameObject);
        }
    }
}
