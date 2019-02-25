using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryOrbController : MonoBehaviour
{


    // Update is called once per frame
    void Update(){
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Hit player");
            collision.gameObject.GetComponent<PlayerController>().GiveVictoryOrb();
            Destroy(gameObject);
        }
    }
}
