using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeAttack : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    int timer = 40;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timer--;
        }
        m_Rigidbody.velocity = transform.forward * 1;
    }

    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
            float thrust = 2;
            if (collision.gameObject.tag == "Pawn")
            {
            Debug.Log("push");
            collision.rigidbody.velocity = transform.forward * thrust;
            Destroy(gameObject);
        }
       
    }
}
