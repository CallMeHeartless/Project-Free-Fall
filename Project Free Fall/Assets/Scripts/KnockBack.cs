using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    float forceStrength = 5.0f;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && other != this) {
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0; // Remove vertical component to knock back
            other.GetComponent<PlayerController>().AddImpulse(forceStrength * direction);
            Debug.Log(forceStrength * direction);
        }
    }

}
