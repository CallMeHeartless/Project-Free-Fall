using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitboxController : MonoBehaviour
{
    float forceStrength = 5.0f;
    [SerializeField]
    AudioSource[] sounds;

    private void OnTriggerEnter(Collider other) {

    
    // Stop player
    PlayerController parent = transform.parent.GetComponent<PlayerController>();
        if(parent == null) {
            Debug.Log("Parent does not exist"); // Can remove if logic is sound
        } else {
            parent.StopPlayer();
        }

        if(other.CompareTag("Player") && other != this) {
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0; // Remove vertical component to knock back
            other.GetComponent<PlayerController>().AddImpulse(forceStrength * direction);
            other.GetComponent<PlayerController>().DamagePlayer(3);
            gameObject.SetActive(false); // Disable on hit
        }
        else if (other.CompareTag("wall"))
        {
            //Debug.Log("hit");
            gameObject.transform.parent.GetComponent<PlayerController>().AddImpulse(new Vector3(0,0,0));
            gameObject.transform.parent.GetComponent<PlayerController>().StunPlayer(3);
            sounds[Random.Range(0, 1)].Play(0);
        }
    }

    public void SetForceStrength(float magnitude) {
        forceStrength = magnitude;
    }
}
