using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitboxController : MonoBehaviour
{
    float forceStrength = 5.0f;

    private void OnTriggerEnter(Collider other) {

        // Stop player
        PlayerController parent = gameObject.GetComponent<PlayerController>();
        if(parent == null) {
            Debug.Log("Parent does not exist"); // Can remove if logic is sound
        } else {
            parent.StopPlayer();
        }

        if(other.CompareTag("Player") && other != this) {
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0; // Remove vertical component to knock back
            other.GetComponent<PlayerController>().AddImpulse(forceStrength * direction);
            Debug.Log(forceStrength * direction);
        }
    }

    public void SetForceStrength(float magnitude) {
        forceStrength = magnitude;
    }
}
