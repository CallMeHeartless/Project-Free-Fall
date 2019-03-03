using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // Spawn a new victory orb if the player died with it
            if (other.GetComponent<PlayerController>().CheckForVictoryOrb()) {
                RoundManager.SpawnVictoryOrb();
            }
            Destroy(other.gameObject);
        }else if (other.CompareTag("VictoryOrb")) {

            other.transform.position = GameObject.Find("VictoryOrbSpawnPoint").transform.position;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
