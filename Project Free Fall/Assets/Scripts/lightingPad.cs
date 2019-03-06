using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightingPad : MonoBehaviour
{
    public float timer;
    public bool On;
    public float onTimerlimit;
    public float cooldown;
    public float stunlength;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        timer = cooldown;
       // gameObject.GetComponent<MeshCollider>().enabled = false;
        On = false;
    }
    
  
  
    // Update is called once per frame
    void Update()
    {
        if (On == false)
        {


            if (timer >= 0)
            {
                timer -= Time.deltaTime;


            }
            else
            {
              //gameObject.GetComponent<MeshCollider>().enabled = true;
                On = true;
                timer = onTimerlimit;
                gameObject.transform.GetChild(0).gameObject.active = true;
            }
        }
        else
        {


            if (timer >= 0)
            {
                timer -= Time.deltaTime;


            }
            else
            {
               // gameObject.GetComponent<MeshCollider>().enabled = false;
                On = false;
                timer = cooldown;
                gameObject.transform.GetChild(0).gameObject.active = false;

            }
            if (player!= null)
            {
                collisioncheck();
            }
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");

        if (collision.gameObject.CompareTag("Player")){
            //gameObject.GetComponent<MeshCollider>().enabled = false;
            On = false;
            timer = cooldown;
            player = collision.gameObject;
        }
        
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("leave");
        player = null;



    }
    void collisioncheck()
    {
        Debug.Log("hte");
        gameObject.transform.GetChild(0).gameObject.active = false;
        player.GetComponent<PlayerController>().StunPlayer(stunlength);
            On = false;
            timer = cooldown;
        player = null;

    } 
}
