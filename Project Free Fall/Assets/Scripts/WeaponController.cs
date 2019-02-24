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
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0.0f;
            other.GetComponent<PlayerController>().AddImpulse(forceStrength * direction);

            // Hit player audio
        } else {
            // Hit object audio
        }

    }

}
