using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bumpers : MonoBehaviour
{
    public float forceStrength = 5.0f;
    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other)
    
        {

        Debug.Log("hitting");
        if (other.CompareTag("Player") && other != this)
        {
            Debug.Log("player");
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0; // Remove vertical component to knock back
            other.GetComponent<PlayerController>().AddImpulse(forceStrength * direction);
            other.GetComponent<PlayerController>().DamagePlayer(3);
            gameObject.GetComponent<Animator>().SetTrigger("Bounce");
        }

       
    }
}
