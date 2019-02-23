using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitboxController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {

        // Stop player
        PlayerController parent = gameObject.GetComponent<PlayerController>();
        if(parent == null) {
            Debug.Log("Parent does not exist"); // Can remove if logic is sound
        } else {
            parent.StopPlayer();
        }

        if(other.CompareTag("Player") && other != this) {
          //  other.GetComponent<PlayerController>().AddImpulse();
        }
    }
}
