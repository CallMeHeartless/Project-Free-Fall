using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("destorying");
            Destroy(collision.gameObject);
            Debug.Log("destoryed");
        }
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
    }

}
