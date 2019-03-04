using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private float forceStrength = 1.0f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // Apply soft knock back
            //Vector3 direction = other.transform.position - transform.parent.position;
            Vector3 direction = transform.root.forward;
            direction.y = 0.0f;
            other.GetComponent<PlayerController>().AddImpulse(forceStrength * direction);
            other.GetComponent<PlayerController>().DamagePlayer(1);

            gameObject.SetActive(false); // Current fix to prevent the damage being triggered twice

            // Hit player audio
            other.GetComponentInChildren<PlayerAudioController>().PlayerHitAudio();
        } else {
            gameObject.SetActive(false);
            other.GetComponentInChildren<PlayerAudioController>().PlayerHitAudio();
            // Hit object audio
        }

    }

}
