using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast : MonoBehaviour
{
    public float Range;
    public float speed;
    public float thrust;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<SphereCollider>().radius<= Range)
        {
            gameObject.GetComponent<SphereCollider>().radius += speed;

        }
        else
        {
            Destroy(gameObject);
        }
      
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");

        if (collision.gameObject.CompareTag("Player"))
        {
            
            //gameObject.GetComponent<MeshCollider>().enabled = false;
            collision.rigidbody.velocity = ((collision.transform.position - transform.position) * thrust) + Quaternion.Euler(0, collision.transform.rotation.y, 0) * -new Vector3(0, Random.Range(0.5f, 1.0f), 0);


            }

    }
   //new Vector3( collision.transform.position - gameObject.transform.position);
}
